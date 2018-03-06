
var Grids = function () {
    var companyId = 0, admin, lang, roleNames, isAllowInsert;
    var custom = {}, pages = [], msg = {};

    (function ($, kendo) {
        $.extend(true, kendo.ui.validator, {
            rules: {
                minlength: function (input) {
                    var min = input.attr("minlength");
                    if (input && input.val() && input.val().length < min) {
                        return false;
                    }

                    return true;
                },
                required: function (input) {
                    var flag = false;
                    var type = input.attr("type");
                    var form = input.parents(".k-grid");
                    if (form.filter("[type='radio']") && type == "radio" && input.attr("required"))
                        return form.find("[type='radio']").is(':checked');

                    if (input.attr("required") == "required") {

                        if (type === "checkbox")
                            return !flag;
                        else {
                            var va = input.val();
                            if (va && va.length > 0)
                                return !flag;
                            else
                                return flag;
                        }
                    }
                    return !flag;
                },
                mvcremotevalidation: function (input) {
                    if (input.is("[data-val-remote]") && input.val() != "") {
                        var remoteURL = input.attr("data-val-remote-url");
                        var name = input.prop("name");
                        var valid;
                        var grid =  $('#' + $(input).closest("[data-role=grid]").attr("id")).data("kendoGrid")
                        var dataSource = grid.dataSource.data(), tr = grid.dataItem($(input).closest("tr"));

                        if (tr[name] == $(input).val()) // no changes
                            return true;

                        var duplicated = dataSource.filter(function (item) { return item[name] == input.val() });
                        if (duplicated.length) return false;

                        $.ajax({
                            async: false,
                            url: remoteURL,
                            type: "POST",
                            dataType: "json",
                            contentType: 'application/json',
                            data: validationData(input, this.element, tr),
                            success: function (result) {
                                if (result == true)
                                    tr[name] = $(input).val();
                                valid = result;
                            },
                            error: function (result) {
                                valid = false;
                            }
                        });

                        return valid;
                    }

                    return true;
                }
            },
            messages: {
                minlength: function (input) {
                    return msg.LengthCantBeLessThan + " " + input.attr("minlength");
                },
               
                mvcremotevalidation: function (input) {
                    return input.attr("data-val-remote");
                },
                required: function (input) {

                    return msg.Required;
                }
            }
        });

        function validationData(input, context, tr) {
            var fields = input.attr("data-val-remote-additionalFields").split(",");
            var name = input.prop("name");
            var prefix = name.substr(0, name.lastIndexOf(".") + 1);
            var fieldName, value;

            var model = {};
            model.columns = [];
            model.values = [];
            model.tablename = $(input).closest("[data-role=grid]").attr("tableName");
            model.modelname = $(input).closest("[data-role=grid]").attr("objectName");


            if (input.attr("data-parentColumn")) {
                model.isLocal = input.attr("data-isLocal");
                model.parentColumn = input.attr("data-parentColumn");
                model.parentId = input.attr("data-parentId");
            } else {
                model.parentColumn = null;
                model.parentId = null;
            }

            tr.Id ? model.id = tr.Id : model.id = null;

            for (var i = 0; i < fields.length; i++) {
                fieldName = fields[i].replace("*.", prefix);
                value = $("[name='" + fieldName + "']", context).val();
                if (value == undefined) value = tr[fieldName];
                model.columns.push(fieldName);
                model.values.push(value);
            }

            return JSON.stringify(model);
        }
    })(jQuery, kendo);

    $.urlParam = function (name) {
        if (ulr !== undefined) {
            var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(ulr);
            if (results) return results[1] || 0; // decodeURI(results[1]) for special char
        }
        return null;
    }

    function findObject(json, key) {
        for (var i = 0, len = json.length; i < len; i++) {
            if (json[i].column === key) {
                return json[i];
            }
        }
    }

    function findInArray(array, key) {
        for (var i = 0, len = array.length; i < len; i++) {
            if ((array[i].field && array[i].field == key) || (array[i].id && array[i].id == key)) {
                return array[i];
            }
        }
    }

    function toDateTime(str)
    {
        var d = new Date(str);
        if (isNaN(d) == false)
            return kendo.toString(d, 'g');

        var indx = str.indexOf('(');
        if (indx >= 0) {
            d = new Date(parseInt(str.substring(indx + 1, str.indexOf(')'))));
            if (isNaN(d) == false)
                return kendo.toString(d, 'g');
        }
        
       return "";
    }

    function toValidJson(record) {
        for (var member in record) {
            if (member.toLowerCase().indexOf("date") != -1) {
                record[member] = kendo.toString(record[member], 'd');
            } else if (member.toLowerCase().indexOf("time") != -1 && record[member]) {
                record[member] = toDateTime(String(record[member]));
            }
        }
        return record.toJSON();
    }

    function sendData(name, url) {
        var grid = $('#' + name).data("kendoGrid");
            //parameterMap = grid.dataSource.transport.parameterMap;

        //get the new and the updated records
        var currentData = grid.dataSource.data();
        var updatedRecords = [];
        var newRecords = [];

        for (var i = 0; i < currentData.length; i++) {
            if (currentData[i].isNew()) {
                newRecords.push(toValidJson(currentData[i]));
            } else if (currentData[i].dirty) {
                updatedRecords.push(toValidJson(currentData[i]));
            }
        }

        //this records are deleted
        var deletedRecords = [];
        for (var i = 0; i < grid.dataSource._destroyed.length; i++) {
            if (grid.dataSource._destroyed[i].Id)
                deletedRecords.push({ Id: grid.dataSource._destroyed[i].Id });
        }

        var viewmodel = {};
        viewmodel.updated = updatedRecords;
        viewmodel.deleted = deletedRecords;
        viewmodel.inserted = newRecords;

        if (url) {
            $.ajax({
                url: url,
                data: viewmodel,
                type: "POST",
                error: function () {
                    //Handle the server errors 
                },
                success: function (data) {
                    Ok(data, name);
                    grid.dataSource._destroyed = [];

                    //refresh the grid - optional
                    grid.dataSource.read();
                }
            });
        } else {
            var dataChanged = $('#' + name).attr("dataChanged");
            if (dataChanged == "true") changed(name, "Data", false);
            return viewmodel;
        }
    }

    function beforeCreateGrid(data, args) {
        //roleNames = (data.RoleNames == "empty" ? [] : JSON.parse(data.RoleNames));
        var columnsTitles = (data.ColumnTitles == "empty" ? [] : JSON.parse(data.ColumnTitles));
        //localStorage[args.gridName + "-IColumns"] = data.HiddenColumns; //(data.HiddenColumns == "empty" ? [] : data.HiddenColumns);
        //localStorage[args.gridName + "-DColumns"] = data.DisabledColumns; //(data.DisabledColumns == "empty" ? [] : data.DisabledColumns);
        var info = data.ColumnInfo.replace(/\$/g, '');
        var dbColumns = (info == "empty" ? [] : JSON.parse(info));
        isAllowInsert = data.IsAllowInsert || false;
        //convert array to object
        var ar = (data.JsMessages == "empty" ? [] : JSON.parse(data.JsMessages));
        for (var i = 0; i < ar.length; i++) {
            msg[ar[i].name] = ar[i].msg;
        }

        var hiddenColumns = data.HiddenColumns ? data.HiddenColumns.split(',') : [];
        
        var userHiddenColumns = [];
        if (admin != "True") {
            var ds = localStorage[args.objectName + "-grid-options"];
            if (ds) {
                ds = JSON.parse(ds);
                if (ds.columnsHidden) {
                    userHiddenColumns = ds.columnsHidden.map(function (item) { return item && (item.field || item.id) }); 
                }
            }
        }

        var disabledColumns = data.DisabledColumns ? data.DisabledColumns.split(',') : [];
        if (args.fields == undefined) args.fields = {};
        if (args.select == undefined) args.select = {};

        if (dbColumns.length > 0) {
            var newColumns = [], k = 0, dataLevel = $.urlParam('DataLevel'), role = $.urlParam('RoleId');
            for (var i = 0; i < dbColumns.length; i++) {
                var column;
                // args.admin == "False" && 
                if (hiddenColumns) {
                    if ($.inArray(dbColumns[i].name, hiddenColumns) >= 0) continue;
                }

                // Create columns
                if (dbColumns[i].input != "none") {
                    column = args.columns && findInArray(args.columns, dbColumns[i].name) || {};
                    if (args.editable === false) dataLevel = 0;

                    if (dbColumns[i].type == "button") {
                        if (column.input == "none") continue;
                        var bname = dbColumns[i].name.toLowerCase();
                        if (((dataLevel == 0) && (bname == "show" || bname == "edit" || bname == "delete")) ||
                           ((dataLevel == 1 || dataLevel == 2) && (bname == "edit" || bname == "delete")) ||
                           ((dataLevel == 3) && (bname == "delete")))
                            continue;

                        column.title = " ";
                        if (bname == "edit") column.command = { name: bname, text: " ", click: editRecord }; //k-sprite fa fa-search
                        else if (bname == "delete") column.command = { name: bname, imageClass: "k-icon k-i-delete", text: " ", click: (args.del || args.destroy ? deleteRecord : deleteClick) };
                        else if (bname == "show") column.command = { name: bname, text: " ", imageClass: "k-icon k-i-search", click: showRecord };
                        else if (column.command == undefined) column.command = { name: dbColumns[i].name, text: msg[dbColumns[i].name] || dbColumns[i].name };
                        column.id = dbColumns[i].name;
                        
                    } else {
                        if (column.field == undefined) column.field = dbColumns[i].name;
                        if (args.gridType == "batch" && dbColumns[i].input && dbColumns[i].input != "none" && column.editor == undefined) column.editor = customEditor(args.gridName, args.select, dataLevel);

                        // filterable
                        if (column.filterable == undefined) column.filterable = {};
                        if (column.filterable.cell == undefined) column.filterable.cell = {};
                        if (column.filterable.cell.suggestionOperator == undefined) column.filterable.cell.suggestionOperator = "contains";

                        if (dbColumns[i].type == "date") if (dbColumns[i].name.toLowerCase().indexOf("datetime") != -1) column.format = "{0:G}"; else if (dbColumns[i].name.toLowerCase().indexOf("time") != -1) column.format = "{0:hh:mm tt}"; else column.format = "{0:d}";
                    }
                   
                    column.width = dbColumns[i].width;
                    var obj = columnsTitles && findInArray(columnsTitles, dbColumns[i].name);
                    if (obj != undefined)
                        column.title = obj.title;
                    else
                        column.title = dbColumns[i].name;

                    if (dbColumns[i].isVisible == false) {
                        if (admin == "True") {
                            column.hidden = true;
                            column.order = k;
                            k++;
                            newColumns.push(column);
                        }
                    } else { 
                        column.order = k;
                        k++;
                        newColumns.push(column);
                    }

                    //hide columns from local storage for not admin users
                    if (admin != "True" && $.inArray(dbColumns[i].name, userHiddenColumns) >= 0) {
                        column.hidden = true;
                    }
                }

                // Create fields
                if (dbColumns[i].type != "button") {
                    var field = args.fields[dbColumns[i].name] || {};
                    if (dbColumns[i].type) field.type = dbColumns[i].type;
                    var disabled = $.inArray(dbColumns[i].name, disabledColumns) >= 0;

                    if (args.gridType != "index") {
                        var attributes = "", pattern = "";
                        if (dbColumns[i].input == "none") {
                            field.editable = false;
                            field.nullable = !dbColumns[i].required;
                        } else {
                            if (column.editor == undefined && (dbColumns[i].isUnique == "True" || dbColumns[i].custom || dbColumns[i].values || dataLevel == 2 || disabled == true)) column.editor = customEditor(args.gridName, args.select, dataLevel);
                            if (field.validation == undefined) column.editor ? attributes = "" : field.validation = {};
                            if (disabled == true) attributes += "readonly";
                            column.editor ? attributes += (dbColumns[i].required ? " required" : "") : field.validation.required = dbColumns[i].required;
                            if (dbColumns[i].max) column.editor ? attributes += " max='" + dbColumns[i].max + "'" : field.validation.max = dbColumns[i].max;
                            if (dbColumns[i].min != undefined) column.editor ? attributes += " min='" + dbColumns[i].min + "'" : field.validation.min = dbColumns[i].min;
                            if (dbColumns[i].maxlength) column.editor ? attributes += " maxlength='" + dbColumns[i].maxlength + "'" : field.validation.maxlength = dbColumns[i].maxlength;
                            if (dbColumns[i].minlength) column.editor ? attributes += " minlength='" + dbColumns[i].minlength + "'" : field.validation.minlength = dbColumns[i].minlength;
                            if (dbColumns[i].placeholder) column.editor ? attributes += " placeholder='" + dbColumns[i].placeholder + "'" : field.validation.placeholder = dbColumns[i].placeholder;
                            if (dbColumns[i].pattern) column.editor ? pattern = dbColumns[i].pattern : field.validation.pattern = dbColumns[i].pattern;
                            if (dbColumns[i].custom && column.editor) attributes += " " + dbColumns[i].custom;
                            if (dbColumns[i].defaultValue) field.defaultValue = dbColumns[i].defaultValue;
                            if (column.editor && dbColumns[i].isUnique == "True") {
                                attributes += " data-val-remote='" + msg.AlreadyExists + "'";
                                attributes += " data-val-remote-url='../../Pages/IsUniqueP'";
                                attributes += " data-val-remote-additionalfields='*." + dbColumns[i].name;
                                if (dbColumns[i].uniqueColumns) {
                                    var array = dbColumns[i].uniqueColumns.split(",");
                                    for (var x = 0; x < array.length; x++)
                                        attributes += ",*." + array[x].trim();
                                }
                                attributes += "'";
                                if (args.parentColumnName) attributes += " data-isLocal= " + (args.isLocal ? "true" : "false") + " data-parentColumn='" + args.parentColumnName + "' data-parentId='" + args.parentColumnId + "'";
                            }

                            if (column.editor) {
                                custom[args.gridName + '-' + dbColumns[i].name] = {};
                                custom[args.gridName + '-' + dbColumns[i].name].type = dbColumns[i].input || "input";
                                custom[args.gridName + '-' + dbColumns[i].name].datatype = dbColumns[i].type;
                                custom[args.gridName + '-' + dbColumns[i].name].attributes = attributes;
                                custom[args.gridName + '-' + dbColumns[i].name].pattern = pattern;
                                custom[args.gridName + '-' + dbColumns[i].name].codeName = dbColumns[i].codeName;
                                custom[args.gridName + '-' + dbColumns[i].name].custom = dbColumns[i].custom;
                            }
                        }
                    }


                    args.fields[dbColumns[i].name] = field;
                }
            }

            args.columns = newColumns;
        } else {
            for (var i = 0; i < args.columns.length; i++)
                args.columns[i].order = i;
        }
    }

    function loadLocalStorage(args) {
        //load local storage
        var x = localStorage[args.objectName + "-grid-options"];
        var db = x && JSON.parse(x);
        if (db == undefined) db = {};
        if (db.filterable == undefined) db.filterable = { mode: "menu" };
        if (db.columnMenu == undefined) db.columnMenu = true;
        if (db.pageSize == undefined) db.pageSize = 10;

        if (admin == "False" && db.columnsOrder && db.columnsOrder.length == args.columns.length) {
            var newColumns = [];
            for (var i = 0; i < args.columns.length; i++) {
                var order = db.columnsOrder ? db.columnsOrder[i] : i;
                if (db.columnsWidth && db.columnsWidth[i]) {
                    args.columns[order].width = db.columnsWidth[i];
                }
                newColumns[i] = args.columns[order];
            }
            args.columns = newColumns;
        }

        return db;
    }

    var indexGrid = function (args) {
        // Initialize arguments
        admin = args.admin || "False";
        // Ajax get sqlserver db info
        $.getJSON("../../Pages/GetGrid", { objectName: args.objectName, version: args.version || $.urlParam('Version') },
            function (d) {

                args.gridType = "index";
                lang = args.lang;
                kendo.culture(lang);
                //if ($.inArray(lang.split('-')[0], ["ps", "ar", "ur", "ku", "fa"]) >= 0) $("body").addClass("k-rtl");
            

                beforeCreateGrid(d, args);
                var db = loadLocalStorage(args);

                if (args.height == undefined) {
                    $('#' + args.gridName).replaceWith('<div id="' + args.gridName + 'splt" style="height: ' + (db.Size ? Number(db.Size) + 100 : 700) + 'px">');
                    $('#' + args.gridName + 'splt').append('<div id="' + args.gridName + '"></div><div></div>');
                }

                var dataSource = (args.dataSource == undefined ? {
                    transport: {
                        read: args.read
                    },
                    schema: {
                        model: {
                            fields: args.fields
                        }
                    },
                    pageSize: args.pageSize || db.pageSize,
                    serverPaging: args.serverPaging || false,
                    serverFiltering: args.serverFiltering || false,
                    serverSorting: args.serverSorting || false,
                    filter: args.filter || db.filter,
                    sort: args.sort || db.sort,
                    group: args.group || db.group,
                    aggregate: args.aggregate
                } : args.dataSource);

                if (args.serverPaging === true) {
                    dataSource.schema.data = "data"; // records are returned in the "data" field of the response
                    dataSource.schema.total = "total"; // total number of records is in the "total" field of the response
                }

                $('#' + args.gridName).kendoGrid({
                    pdf: {
                        allPages: true,
                        avoidLinks: true,
                        paperSize: "A4",
                        margin: {
                            top: "2cm",
                            left: "1cm",
                            right: "1cm",
                            bottom: "1cm"
                        },
                        landscape: true,
                        repeatHeaders: true,
                        template: $("#page-template").html(),
                        scale: 0.5
                    },
                    dataSource: dataSource,
                    height: args.height || db.Size || 700,
                    //toolbar: args.toolbar || kendo.template($("#template").html()),
                    scrollable: args.scrollable || {
                        virtual: true
                    },
                    excel:{
                        allPages: true,
                        fileName:"ExportXsl.xlsx"
                    },
                    groupable: (args.groupable == undefined ? true : args.groupable),
                    sortable: (args.sortable == undefined ? true : args.sortable),
                    reorderable: (args.reorderable == undefined ? true : args.reorderable),
                    pageable: (args.pageable == undefined ? { refresh: true, pageSizes: true, buttonCount: 3 } : args.pageable), //{ refresh: true, numeric: args.pageable, previousNext: args.pageable, message: { display: "{2} " + msg.rows } }),   //for number of elements if not pagable
                    filterable: (args.filterable == undefined ? db.filterable : args.filterable),
                    columnMenu: (args.columnMenu == undefined ? db.columnMenu : args.columnMenu),
                    resizable: true,
                    detailInit: args.detailInit,
                    columns: args.columns,
                    selectable: args.selectable,
                    change: args.change,
                    columnHide: columnsHide(args.gridName),
                    columnResize: columnReorder(args.gridName),
                    columnReorder: columnReorder(args.gridName),
                    columnShow: columnsHide(args.gridName),
                    columnMenuInit: menuInit(args.gridName),
                    dataBound: (args.dataBound ? function (e) { dataBound(args.gridName); args.dataBound(e) } : function (e) { dataBound(args.gridName)})
                });

                $('#' + args.gridName).attr("editUrl", args.edit);
                $('#' + args.gridName).attr("deleteUrl", args.del);
                $('#' + args.gridName).attr("recordName", args.recordName);
                $('#' + args.gridName).attr("objectName", args.objectName);
                $('#' + args.gridName).attr("tableName", args.tableName || args.objectName);
                $('#' + args.gridName).attr("dataChanged", "false");
                $('#' + args.gridName).attr("designChanged", "false");
                $('#' + args.gridName).attr("gridType", args.gridType);

                redrawGrid(0, args.gridName, 'index');

                $(".glyphicon-plus").hover(function (e) {
                    $(this).toggleClass("btn-default").toggleClass("btn-success");
                });

                $(".glyphicon-cog").hover(function (e) {
                    $(this).toggleClass("btn-default").toggleClass("btn-primary");
                });

                $('#' + args.gridName).ready(function () {
                    gridReady(db, args.gridName);
                });
            });
    }

    function gridReady(db, name) {
        if (document.getElementById(name + 'splt') != null) {
            var splitter = $('#' + name + 'splt');
            splitter.kendoSplitter({
                orientation: "vertical", panes: [{ collapsible: false, size: db.Size ? db.Size : 600 }, { collapsible: true }],
                resize: function (e) {
                    var size = String(splitter.data("kendoSplitter").size(".k-pane:first"));
                    var index = size.indexOf('.');
                    if (index == -1) index = size.indexOf('px');
                    if (index == -1) index = size.length;
                    db.Size = Number(size.substr(0, index));
                    localStorage[$('#' + name).attr("objectName") + "-grid-options"] = JSON.stringify(db);
                }
            });

            if (splitter.parent().hasClass('tab-pane') === true) {
                setTimeout(function () { splitter.data("kendoSplitter")._resize(); }, 410);
            }
        }
    }

    function Ok(data, name) {
        var result = JSON.parse(data.responseText);
        var message = "";

        if (result.Errors) {
            for (var i = 0; i < result.Errors.length; i++) {
                for (var k = 0; k < result.Errors[i].errors.length; k++) {
                    message += result.Errors[i].errors[k].message;
                }
            }
        }
        if (message.length === 0) {
            $('#' + name).attr("hasErrors", "false");
            var dataChanged = $('#' + name).attr("dataChanged");
            if (dataChanged == "true") {
                changed(name, "Data", false);
                toastr.success(msg.SaveComplete);
            } else {
                $('#' + name).attr("hasErrors", "true");
                toastr.error(message.replace(/;/g, "<br/>"));
            }
        }
    }

    function setErrors(parms, name) {
        if (parms.errors) {
            var grid = $(name).data("kendoGrid")
            //remove previous errors
            grid.table.find(".errorCell").each(function () {
                $(this).removeClass("errorCell");
            })

            grid.one("dataBinding", function (e) {
                e.preventDefault();   // cancel grid rebind if error occurs                             

                var fieldCellIndices = {},
                dataSource = grid.dataSource,
                errors = parms.errors;
                      
                //get current column indexes
                for (var i = 0; i < grid.columns.length; i++) {
                    if (grid.columns[i].field) {
                        fieldCellIndices[grid.columns[i].field] = i + (grid._events.detailInit ? 1 : 0);
                    }
                }

                pages = [];

                for (var i = 0; i < errors.length; i++) {
                    var error = errors[i];
                    var item;


                    var obj = findInArray(pages, error.page) || {};
                    if (obj.page !== undefined)
                        obj.count++;
                    else {
                        obj.page = error.page;
                        obj.count = 1;
                        pages.push(obj);
                    }

                    if (error.id && error.id != "0")
                        item = dataSource.get(error.id);
                    else if (error.row !== undefined)
                        item = dataSource.at(error.row);
                    else
                        break;

                    var row = grid.table.find("tr[data-uid='" + item.uid + "']");

                    for (var j = 0; j < error.errors.length; j++) {
                        var field = error.errors[j].field,
                            message = error.errors[j].message.replace(/;/g, "<br/>");

                        //find the cell
                        var container = row.find("td:eq(" + fieldCellIndices[field] + ")");

                        //show the validation error message
                        container.append(kendo.template('<div class="k-widget k-tooltip k-tooltip-validation k-invalid-msg field-validation-error hidden" style="margin: 0.5em; display: block; " data-for="#=field#" data-valmsg-for="#=field#" id="#=field#_#=id#_validationMessage"><span class="k-icon k-warning"> </span>#=message#<div class="k-callout k-callout-n"></div></div>')({ field: field, message: message, id: i }));
                        //highlight the cell that have error
                        container.addClass("errorCell");
                      //  container.css("border-color:red");
                    }
                }
            });

        }
    };

    var batchGrid = function (args) {

        // Initialize
        admin = args.admin || "False";
       
        function _Ok(data) {
            Ok(data, args.gridName);
            if (args.onCompleted) args.onCompleted();
        }

        function errorHandler(name) {
            return function (parms) {
                setErrors(parms, name);
            };
        }

        // Ajax get sqlserver db info
        if (args.objectName) {
            $.getJSON("../../Pages/GetGrid", { objectName: args.objectName, version: args.version || $.urlParam('Version') || 0 },
                function (d) {
                    beforeCreateGrid(d, args);
                    drawGrid();
                });
        } else drawGrid();

        function drawGrid() {
            lang = args.lang;
            kendo.culture(lang);
            //if ($.inArray(lang.split('-')[0], ["ps", "ar", "ur", "ku", "fa"]) >= 0) $("body").addClass("k-rtl");
            args.gridType = "batch";
            var db = loadLocalStorage(args);


            if (args.height == undefined) {
                $('#' + args.gridName).replaceWith('<div id="' + args.gridName + 'splt" style="height: ' + (db.Size ? Number(db.Size) + 75 : 675) + 'px">');
                $('#' + args.gridName + 'splt').append('<div id="' + args.gridName + '"></div><div></div>');
            }

            var transport = {
                read: {
                    url: args.read,
                    dataType: "json", // "jsonp" is required for cross-domain requests; use "json" for same-domain requests
                    contentType: "application/json",
                    complete: (args.readCompleted ? args.readCompleted : undefined)
                },
                parameterMap: function (data, type) {
                    if (type !== "read" && data.models) {
                        var result = {}; // define result as jave script object
                        for (var i = 0; i < data.models.length; i++) {
                            var record = data.models[i];
                            if (record.Id == null) record.Id = 0;
                            record[args.parentColumnName] = args.parentColumnId;

                            var hasValues = false;
                            for (var member in record) {
                                if (member == "NewValues" || member == "OldValues") {
                                    hasValues = true;
                                    result["options" + "[" + i + "]." + member] = record[member];
                                } else if (member.toLowerCase().indexOf("date") != -1) {
                                    result["models" + "[" + i + "]." + member] = kendo.toString(record[member], 'd');
                                } else if (member.toLowerCase().indexOf("time") != -1 && record[member]) {
                                    result["models" + "[" + i + "]." + member] = toDateTime(String(record[member]));
                                } else
                                    result["models" + "[" + i + "]." + member] = record[member];
                            }

                            if (!hasValues) {
                                result["options" + "[" + i + "].NewValues"] = { ColumnName: 'NoColumn' };
                                result["options" + "[" + i + "].OldValues"] = { ColumnName: 'NoColumn' };
                            }
                        }
                        return result;
                    }
                }
            };

            if (args.update) {
                transport.update = {
                    url: args.update,
                    dataType: "json",
                    type: "POST",
                    complete: _Ok
                };
            }

            if (args.create) {
                transport.create = {
                    url: args.create,
                    dataType: "json",
                    type: "POST",
                    complete: _Ok
                };
            }

            if (args.destroy) {
                transport.destroy = {
                    url: args.destroy, //specify the URL which should destroy the records. This is the Destroy method of the HomeController.
                    type: "POST", //use HTTP POST request as by default GET is not allowed by ASP.NET MVC
                    complete: _Ok
                };
            }

            var dataLevel = $.urlParam('DataLevel');
            //var read = $.urlParam('Read') || 0;

            if (args.gridName == "ColumnProp") { dataLevel = 2; }  //{ dataLevel = 2; read = 0; }
            $('#' + args.gridName).kendoGrid({
                dataSource: (args.dataSource === undefined ? new kendo.data.DataSource({
                    transport: transport,
                    batch: true,
                    pageSize: args.pageable === false ? 1000 : db.pageSize,
                    filter: args.filter,
                    schema: {
                        errors: "Errors",
                        model: {
                            id: "Id",
                            fields: args.fields
                        }
                    },
                    scrollable: {
                        virtual: true
                    },
                    error: errorHandler('#' + args.gridName),
                    sync: function (e) {
                        var name = '#' + $(e.currentTarget).closest("[data-role=grid]").attr("id");

                        for (var i = 0; i < pages.length; i++) {
                            $(name + " .k-grid-pager > ul li:eq(" + pages[i].page + ")")
                                .addClass("relative-pos")
                                .append("<span class='badge'>" + pages[i].count + "</span>");
                        }
                        pages = [];
                    }
                }) : new kendo.data.DataSource({
                    data: args.dataSource,
                    batch: true,
                    pageSize: args.pageable === false ? 1000 : db.pageSize,
                    filter: args.filter,
                    schema: {
                        errors: "Errors",
                        model: {
                            id: "Id",
                            fields: args.fields
                        }
                    },
                    scrollable: {
                        virtual: true
                    },
                    error: errorHandler('#' + args.gridName),
                    sync: function (e) {
                        var name = '#' + $(e.currentTarget).closest("[data-role=grid]").attr("id");

                        for (var i = 0; i < pages.length; i++) {
                            $(name + " .k-grid-pager > ul li:eq(" + pages[i].page + ")")
                                .addClass("relative-pos")
                                .append("<span class='badge'>" + pages[i].count + "</span>");
                        }
                        pages = [];
                    }
                })),
                serverPaging: args.serverPaging || false,
                serverFiltering: args.serverFiltering || false,
                serverSorting: args.serverSorting || false,
                navigatable: (args.navigatable == undefined ? true : args.navigatable),
                sortable: (args.sortable == undefined ? true : args.sortable),
                reorderable: (args.reorderable == undefined ? true : args.reorderable),
                pageable: (args.pageable == undefined ? { refresh: true, pageSizes: true, buttonCount: 3 } : { numeric: args.pageable, previousNext: args.pageable, messages: { display: "{2} " + msg.rows } }),  //for number of elements if not pagable
                filterable: (args.filterable == undefined ? db.filterable || { mode: "menu,row" } : args.filterable),
                columnMenu: (args.columnMenu == undefined ? db.columnMenu : args.columnMenu),
                resizable: true,
                height: args.height || db.Size,
                toolbar: args.toolbar === "<div></div>" ? undefined : (args.toolbar || kendo.template($("#template").html())),
                columns: args.columns,
                //pdf: {
                //    allPages: true,
                //    fileName: "ExportPdf.pdf"
                //    avoidLinks: true,
                //    paperSize: "A4",
                //    margin: { top: "2cm", left: "1cm", right: "1cm", bottom: "1cm" },
                //    landscape: true,
                //    repeatHeaders: true,
                //    template: $("#page-template").html(),
                //    scale: 0.8
                //},
                excel: {
                    allPages: true,
                    fileName: "ExportXsl.xlsx"
                },
                //pdfExport: function (e) {
                //    e.promise.done(function () {
                //        $("#" + args.gridName).find(".k-grid-toolbar").css("display", "");
                //        $("#" + args.gridName).find(".k-grouping-header").css("display", "");
                //    });
                //},
                detailInit: args.detailInit,
                detailExpand: args.detailExpand,
                detailCollapse: args.detailCollapse,
                columnHide: columnsHide(args.gridName),
                columnResize: columnReorder(args.gridName),
                columnReorder: columnReorder(args.gridName),
                columnShow: columnsHide(args.gridName),
                selectable: args.selectable,
                dataBound: (args.dataBound ? function (e) { dataBound(args.gridName); args.dataBound(e); } : dataBound(args.gridName)),
                saveChanges: args.saveChanges,
                save: function (e) {
                    e.model.Page = e.sender.dataSource.page();

                    var dataChanged = $('#' + args.gridName).attr("dataChanged");
                    if (dataChanged != "true") changed(args.gridName, "Data", true);

                    if (args.save) args.save(e);
                },
                columnMenuInit: (args.objectName ? menuInitB(args.gridName) : undefined),
                editable: (args.editable === true ? true : dataLevel < 2 ? false : true), // (read == 1 || dataLevel < 2 || args.editable === false ? false : true),
                change: args.change
            });

            $('#' + args.gridName).attr("objectName", args.objectName);
            $('#' + args.gridName).attr("tableName", args.tableName || args.objectName);
            $('#' + args.gridName).attr("recordName", args.recordName);
            $('#' + args.gridName).attr("deleteUrl", args.destroy);
            $('#' + args.gridName).attr("hasErrors", "false");
            $('#' + args.gridName).attr("dataChanged", "false");
            $('#' + args.gridName).attr("designChanged", "false");
            $('#' + args.gridName).attr("gridType", args.gridType);


            redrawGrid(0, args.gridName, 'batch');

            $(".glyphicon-plus").hover(function (e) {
                $(this).toggleClass("btn-default").toggleClass("btn-success");
            });

            $(".glyphicon-ok").hover(function (e) {
                $(this).toggleClass("btn-default").toggleClass("btn-primary");
            });

            $(".glyphicon-ban-circle").hover(function (e) {
                $(this).toggleClass("btn-default").toggleClass("btn-warning");
            });

            $(".glyphicon-cog").hover(function (e) {
                $(this).toggleClass("btn-default").toggleClass("btn-primary");
            });
            $('#' + args.gridName).on('hover', ".errorCell", function (e) {
                if (e.type != "mouseenter")
                    $(this).find("div.k-invalid-msg").addClass('hidden');
                else
                    $(this).find("div.k-invalid-msg").removeClass('hidden');
            });

            $('#' + args.gridName).ready(function () {
                gridReady(db, args.gridName);
            });

            IntialNotifyWind(args.gridName);
        }


        $("#" + args.gridName).on("change", "input[type='checkbox']", null, function () {
            var gridName = $(this).parent().closest(".k-grid").attr("id");
            var dataChanged = $('#' + gridName).attr("dataChanged");
            if (dataChanged != "true") changed(gridName, "Data", true);
        });

    }
  
    var orgClickHandler;
    function changed(gridName, mode, on) {
        var gridclose = $("#" + gridName).prev().children("button.close");
        var gridType = $("#" + gridName).attr("gridType");
        if (gridType == "batch")
            var selectors = $("a, :button").not("#" + gridName + " :button, #" + gridName + " a, [href='javascript:;'],button.close, .addOptionG, .ajaxBtn").add(gridclose).add("#toolsMenu a").add("a.k-plus").not("a.glyphicon-floppy-save,:button.submit,.k-animation-container a");
        else
            var selectors = $("a, :button").not("#" + gridName + " :button, #" + gridName + " a, [href='javascript:;'],button.close, .addOptionG, .ajaxBtn").add(gridclose).add("div.k-grid-toolbar a").add("a.k-grid-edit").add("a.k-plus").add("a.k-grid-show").not("a.glyphicon-floppy-save,:button.submit,.k-animation-container a");

        var message = mode + (msg.warningSave == undefined ? " Changed" : msg.warningSave);
        var modeFlag, otherFlag;

        if (mode == "Data") {
            $('#' + gridName).attr("dataChanged", on);
            modeFlag = $('#' + gridName).attr("dataChanged");
            otherFlag = $('#' + gridName).attr("designChanged");
        } else {
            $('#' + gridName).attr("designChanged", on);

            modeFlag = $('#' + gridName).attr("designChanged");
            otherFlag = $('#' + gridName).attr("dataChanged");
        }

        function clickHandler(e) {

            otherFlag = mode == "Data" ? $('#' + gridName).attr("designChanged") : $('#' + gridName).attr("dataChanged");

            var result = confirm(message);
            if (result) {
                e.preventDefault();
                e.stopPropagation();
                selectors.off("click", clickHandler);
                if (otherFlag == "false") {
                    onbeforeunload = null;
                    EventsBack();
                    $(this).click();
                }
                mode == "Data" ? $('#' + gridName).attr("dataChanged", "false") : $('#' + gridName).attr("designChanged", "false");
            } else {
                e.preventDefault();
                e.stopPropagation();
            }
        }

        if (modeFlag == "false") {
            if (mode == "Design")
                mode = "Data";
            onbeforeunload = null;
            selectors.off("click");

            if (otherFlag == "false") EventsBack();
            else {
                changed(mode, selectors);
            }
        }
        else {
            if (otherFlag == "false") {
                //onbeforeunload = function (event) {
                //    event.returnValue = msg;
                //    return msg;
                //};
                if (orgClickHandler == null) {
                    orgClickHandler = [];

                    for (var i = 0; i < selectors.length; i++) {
                        if ($(selectors[i]).data("events") !== undefined) {
                            var funArr = [];

                            var clicks = $(selectors[i]).data("events").click;
                            if (clicks) {
                                for (var j = 0; j < clicks.length; j++) {

                                    funArr.push(clicks[j].handler);
                                }
                            }
                            orgClickHandler[i] = funArr;

                        } else {
                            orgClickHandler[i] = [];
                        }
                    }
                }
                selectors.off("click");
            }
            $(selectors).on("click", clickHandler);
        }

        function EventsBack() {
            for (var i = 0; i < selectors.length; i++) {
                if (orgClickHandler[i]) {
                    for (var j = 0; j < orgClickHandler[i].length; j++) {
                        $(selectors[i]).click(orgClickHandler[i][j]);
                    }
                }
            }
        }
    }

    function IntialNotifyWind(name) {
        //for notification
        var gridDiv = $('#' + name);
        gridDiv.after('<div id="notify-' + gridDiv.attr("objectname") + '" ></div>');
        $("#notify-" + gridDiv.attr("objectname")).kendoWindow({
            title: (msg.Notification ? msg.Notification : "Notifications"),
            minWidth: "500px",
            width: "55%",
            height: "90%",
            actions: ["Minimize", "Maximize", "Close"],
            visible: false,
            close: function () {
                $("#notify-" + gridDiv.attr("objectname")).empty();
            }
        });
    }

    function menuInitB(name) {
        return function (e) {
            var menu = e.container.find(".k-menu").data("kendoMenu"), gridDiv = $("#" + name);
            menu.append('<li class="k-item k-state-default" role="menuitem" id="addNotifyMenu" ><span class="k-link notify"><span class="fa fa-bell"></span>' + (msg.notify ? msg.notify : 'Add Notification') + '</span></li>');

            addRenameLink(name, e);

            e.container.find(".notify").click(function () {
                var columnName = e.field, objectName = gridDiv.attr("objectname");
                $("#notify-" + objectName).data("kendoWindow").refresh("/Notification/NotificationMenu?TableName=" + gridDiv.attr("tablename") + "&ObjectName=" + objectName + "&ColumnName=" + columnName + "&Version=" + $.urlParam('Version') + "&Type=Grid").center().open();
            });
        }
    }

    function addRenameLink(name, e)
    {
        if (admin == "True") {
            $(e.container).find("ul > li.k-sort-desc").append('<li id="renameCell" class="k-item k-state-default" role="menuitem"><span class="k-link rename"><span class="fa fa-edit"></span> ' + msg.RenameColumn + '</span></li>');
            e.container.find(".rename").click(function () {
                var selector = $("#" + name + " thead").find("[data-field='" + e.field + "'] .k-link");
                selector.attr('contenteditable', 'true');
                selector.focus();
                selector.addClass("k-input");

                selector.focusout(function () {
                    $(this).parent().attr("data-title", $(this).text());
                    $(this).removeClass("k-input");
                });
            });
        }
    }

    function menuInit(name) {
        return function (e) {
            addRenameLink(name, e);
        }
    }
    
    //function menuInit(name) {
    //    return function (e) {
    //        var menu = e.container.find(".k-menu").data("kendoMenu");
    //        var field = e.field;
    //        if (admin == "True" && name != "ColumnProp") {
    //            var unCheckRole = false;
    //            var db = localStorage[name + "-IColumns"];
    //            var obj = {};
    //            if (db && db.length > 0) {
    //                var hiddenColumns = JSON.parse(db);
    //                if (hiddenColumns) {
    //                    obj = findInArray(hiddenColumns, field);
    //                    unCheckRole = obj && (obj.roles ? true : false);
    //                }
    //            }
    //            var onChange = "Grids.cbChanged(this, '" + name + "')";
    //            // Option 1: use the kendoMenu API ...
    //            if (roleNames.length > 0) {
    //                for (var i = 0; i < roleNames.length; i++) {
    //                    menu.append('<li class="k-item k-state-default" role="menuitem"><input id="li_' + i + '" roleId= "' + roleNames[i].id + '" field="' + field + '" type="checkbox" ' + (unCheckRole && obj.roles.indexOf(roleNames[i].id) > -1 ? "" : "checked") + ' onchange="' + onChange + '"/><label style="cursor:pointer" for="li_' + i + '">&nbsp; ' + roleNames[i].name + '</label></li>');
    //                }
    //            }
    //        }
    //    };
    //}
    //function cbChanged(e, name) {
    //    var db = localStorage[name + "-IColumns"];
    //    var hiddenColumns = db ? JSON.parse(db) : [];
    //    var element = $(e);
    //    var field = element.attr('field');
    //    var obj = findInArray(hiddenColumns, field);
    //    if (obj == undefined) {
    //        obj = {};
    //        obj.column = field;
    //        obj.roles = [];
    //        hiddenColumns.push(obj);
    //    }
    //    if (element.attr('checked') == 'checked') {
    //       // element.attr("checked", "checked");
    //        if (element.attr('roleId') !== undefined) {
    //            var valueremoved = element.attr('roleId');
    //            obj.roles = $.grep(obj.roles, function (value) { return value != valueremoved; });
    //        }
    //    } else {
    //        //element.attr('checked', 'checked');
    //        var index = element.attr('roleId');
    //        if (index !== undefined) obj.roles.push(index);
    //    }
    //    var designChanged = $('#' + name).attr("designChanged");
    //    if (designChanged == "false")
    //        changed(name, "Design", true);
    //    localStorage[name + "-IColumns"] = JSON.stringify(hiddenColumns);
    //}

    function CheckBoxChanged(el, columnName, gridName) {
      
        var dataItem = $("#" + gridName).data("kendoGrid").dataItem($(el).closest("tr"));
        dataItem[columnName] = $(el).is(':checked');
        dataItem.dirty = true;
    }


    function columnReorder(name) {
        return function (e) {
            if (admin == "False") {
                e.preventDefault();
                var grid = e.sender;
                var key = $('#' + name).attr("objectName");
                var columns = grid.columns;
                var columnsWidth = [];

                // read db key // 
                var db = localStorage[key + "-grid-options"];
                var newOptions = db ? JSON.parse(db) : {};
                var columnsOrder = newOptions.columnsOrder && newOptions.columnsOrder.length == columns.length ? newOptions.columnsOrder : [];


                // get columns order
                for (var i = 0; i < columns.length; i++) {
                    columnsOrder[i] = columns[i].order;
                    columnsWidth[i] = columns[i].width;
                }

                // reorder array
                    columnsOrder.splice(e.newIndex, 0, columnsOrder.splice(e.oldIndex, 1)[0]);
                    columnsWidth.splice(e.newIndex, 0, columnsWidth.splice(e.oldIndex, 1)[0]);
          

                // Save Changes
                newOptions["columnsOrder"] = columnsOrder;
                newOptions["columnsWidth"] = columnsWidth;
                localStorage[key + "-grid-options"] = JSON.stringify(newOptions);
                var designChanged = $('#' + name).attr("designChanged");
                if (designChanged == "false" && admin == "True") changed(name, "Design", true);
            }
        }
    }

    function dataBound(name) {
        return function (e) {
            var grid = e.sender;
            var options = grid.getOptions();
            var key = $('#' + name).attr("objectName");
            var db = localStorage[key + "-grid-options"];
            var newOptions = db ? JSON.parse(db) : {};
            newOptions["filterable"] = options.filterable;
            newOptions["columnMenu"] = options.columnMenu;
            newOptions["sort"] = options.dataSource.sort;
            newOptions["group"] = options.dataSource.group;
            newOptions["filter"] = options.dataSource.filter;
            newOptions["pageSize"] = grid.dataSource.pageSize();

            localStorage[key + "-grid-options"] = JSON.stringify(newOptions);

            $(".k-grid-Delete").hover(function (e) {
                $(this).attr("style", "background-color: red; color: white;");
            }, function (e) {
                $(this).removeAttr("style");
            });
        }
    }

    function deleteClick(e) {
        e.preventDefault();
        e.stopPropagation();
        $(e.currentTarget).clearQueue();
        var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
        var grid = $(e.currentTarget).closest("[data-role=grid]");
        var dataChanged = grid.attr("dataChanged");
        if (dataChanged == "false") changed(grid.attr("id"), "Data", true);

        bootbox.confirm(msg.PermanentlyDeleted.replace("{0}", dataItem[$(grid).attr("recordName")]),
            function (result) {
                if (result) {
                    $('#' + $(grid).attr("id")).data("kendoGrid").dataSource.remove(dataItem);

                }
            });
    }

    function addNewClick(name) {
        var dataLevel = $.urlParam('DataLevel');
        if (dataLevel < 2) return;
        var grid = $('#' + name).data("kendoGrid");
        if (grid.dataSource.at(0)) grid.dataSource.page(1);
        grid.addRow();
    }

    function saveClick(name) {
        $('#' + name).data("kendoGrid").saveChanges();

    }


    function cancelClick(name) {
        var dataChanged = $('#' + name).attr("dataChanged");
        if (dataChanged == "true") changed(name, "Data", false);
        $('#' + name).data("kendoGrid").cancelChanges();
    }

    function customEditor(name, select, dataLevel) {
        return function (container, options) {
            if ((dataLevel == 2 && options.model.isNew() == false) || custom[name + '-' + options.field].attributes.indexOf("readonly") != -1) {
                $('#' + name).data("kendoGrid").closeCell();
                return;
            }

            // create an input element
            var cust = " name='" + options.field + "' class='form-control' style='width: 90%; margin-bottom: 0;' " + custom[name + '-' + options.field].attributes;
            var input, pattern = custom[name + '-' + options.field].pattern ? " pattern='" + custom[name + '-' + options.field].pattern + "'" : "";

            if (custom[name + '-' + options.field].type == "hidden") {
                input = "<input type='hidden' name='" + options.field + "'/>";
            } else if (custom[name + '-' + options.field].type == "textarea") {
                input = "<textarea type='text'" + cust + pattern + "></textarea>";
            //} else if (custom[name + '-' + options.field].type == "select") {
            //    input = "<select name='" + options.field + "' style='width: 80%; margin-bottom: 0;' " + custom[name + '-' + options.field].attributes + ">";
            //    for (var i = 0; i < select[options.field].length; i++)
            //        input += "<option value='" + select[options.field][i].value + "'>" + select[options.field][i].text + "</option>";
            //    input += "</select>";
            } else if (custom[name + '-' + options.field].type == "radio") {
                input = "";
                cust = " name='" + options.field + "' " + custom[name + '-' + options.field].attributes;
                for (var i = 0; i < select[options.field].length; i++)
                    input += "<input type='radio' value='" + select[options.field][i].value + "'" + cust + "/>" + select[options.field][i].text + "<br>";
            } else if (custom[name + '-' + options.field].type == "editmask") {
                input = "<input type='text'" + cust + "/>";
            } else if (custom[name + '-' + options.field].type == "editmask") {
                input = "<input type='text'" + cust + "/>";
            } else {
                var datatype = 'text';

                if (custom[name + '-' + options.field].datatype == 'string') datatype = 'text'
                else if (custom[name + '-' + options.field].datatype == 'number') datatype = 'number'
                else if (custom[name + '-' + options.field].type == 'email') datatype = 'email'
                else if (custom[name + '-' + options.field].type == 'url') datatype = 'url'
                else if (custom[name + '-' + options.field].datatype == 'button') datatype = 'button'
                else if (custom[name + '-' + options.field].datatype == 'boolean') datatype = 'checkbox'
                else if (custom[name + '-' + options.field].datatype == 'date') datatype = 'date'
                else if (custom[name + '-' + options.field].datatype == 'time') datatype = 'time'

                var codeName, objectName;
                if (custom[name + '-' + options.field].codeName) codeName = custom[name + '-' + options.field].codeName;
                if (custom[name + '-' + options.field].custom) {
                    objectName = custom[name + '-' + options.field].custom.indexOf("object") == 0 ? custom[name + '-' + options.field].custom.split("=")[1] : null;
                }

                input = "<input type='" + datatype + "'" + cust + pattern + "/>";
            }

            // append it to the container
            container.append($(input));
            input = container.find("[name='" + options.field + "']");

            if (custom[name + '-' + options.field].type == "editmask") {
                input.kendoMaskedTextBox({
                    mask: custom[name + '-' + options.field].pattern
                });
            }
            else if (custom[name + '-' + options.field].type == "autocomplete") {
                input.kendoAutoComplete({
                    dataSource: select[options.field]
                });
            }
            else if (custom[name + '-' + options.field].datatype == "time") {
                input.kendoTimePicker({
                     culture: lang
                });
            } else if (custom[name + '-' + options.field].type == "select") {
                var grid = $('#' + name).data("kendoGrid");
                var columns = findInArray(grid.columns, $(input).attr("name")); 

                var isAllowed = isAllowInsert && (codeName != undefined || objectName != undefined);
                var currentRow = grid.dataItem($(input).closest("tr")).uid;

                //template
                var params = '"' + name + '","' + codeName + '","' + objectName + '","' + $(input).attr("name") + '","' + currentRow + '"';
                var noData = "<div> " + msg.NoDataFound + (isAllowed ? " <input type='button' class='k-button addOption addOptionG' onclick='Grids.addOption(this," + params + ")' value='" + msg.AddNew + "' />" : "") + "</div>";
                var oldValue = options.model[options.field];
                var activeOptionList = [], list = columns.values ? columns.values : select[options.field];
                if (list) {
                    activeOptionList = list.filter(function (element) { return (element.isActive != false) });
                    if (list && options.model[options.field] != undefined && $.inArray(options.model[options.field], activeOptionList) == -1) {
                        var obj = list.filter(function (e) { return options.model[options.field] && e.isActive == false; });
                        if (obj && obj.length) activeOptionList.push(obj[0]);
                    }
                }

                input.kendoDropDownList({
                    valuePrimitive: true,
                    dataTextField: "text",
                    dataValueField: "value",
                    dataSource: activeOptionList,
                    optionLabel: " ",
                    filter: input.parents(".modal").length ? "none" : "contains", //for popup
                    noDataTemplate: noData,
                    change: function (e) {
                        //initialize
                        if (!options.model.OldValues) options.model.OldValues = [];
                        if (!options.model.NewValues) options.model.NewValues = [];

                        //New Values
                        var isExists = false;
                        for (var i = 0; i < options.model.NewValues.length; i++) {
                            if (options.model.NewValues[i].ColumnName == options.field) {
                                options.model.NewValues[i].Text = e.sender.text();
                                isExists = true;
                                break;
                            }
                        }
                        if (!isExists) { //to prevent duplication 
                            options.model.NewValues.push({ ColumnName: options.field, Text: e.sender.text() });
                            //Old Values
                            var list = e.sender.options.dataSource;
                            for (var j = 0; j < list.length; j++) {
                                if (list[j].value == oldValue) {
                                    options.model.OldValues.push({ ColumnName: options.field, Text: list[j].text });
                                    break;
                                }
                            }
                        }
                    }
                });

                //Kendo window
                $("#" + name).append('<div id="' + name + '-addSelectWindow"></div>');
                $("#" + name + "-addSelectWindow").kendoWindow({
                    title: msg.AddNew,
                    actions: ["Minimize", "Maximize", "Close"],
                    minWidth: "30%",
                    visible: false,
                    close: function () {
                        $("#" + name + "-addSelectWindow").empty();
                    }
                });
            }
        }
    }

    var addOption = function (e, name, codeName, objectName, input, uid) {
        var windowBody = $("#" + name + "-addSelectWindow");
        var newVal = $(e).parents(".k-nodata").prevAll(".k-list-filter").children(".k-textbox").val();
        if (newVal != "") {
            var formData = {};
            formData._Name = newVal;

            if (codeName != "undefined") {
                formData.IsLookUp = 1;
                formData.SourceName = codeName;
                saveOption();
            }
            else {
                formData.IsLookUp = 3;
                if (objectName != "undefined") {
                    formData.SourceName = objectName;
                    $.get("/Pages/GetColumns", { objectname: objectName }, function (result) {
                        var data = result.columns;
                        var fields = [];
                        for (var i = 0; i < data.length; i++) {
                            if (data[i].name == undefined) fields.push(castGridColumns(data[i])); //if Grid
                        }

                        var qualGroupLst = objectName == "Qualifications" ? result.qualGroupLst : undefined;

                        DrawFormWindow(data, qualGroupLst);
                    });
                }
            }

            function DrawFormWindow(data, qualGroupLst) {
                var tableName = data.length ? data[0].TableName : "";
                var formObject = { "FormId": "selectModel", "TableName": tableName, "HasPanel": false, "isDynamic": true, "FieldSets": [{ "Sections": [{ "name": "nameSec", "layout": "form-horizontal", "fields": data }] }] };
                windowBody.Forms(formObject);

                if (qualGroupLst) FormJs.fillOptionsDynamic($(windowBody).find("#selectModel"), "QualGroupId", qualGroupLst, null, {hasFilter: false});
                
                var modalMarkup = "<button class='btn btn-primary submit addOptionG' id='saveAddOption'>" + msg.Save + "</button></div>";
                windowBody.find("#selectModel .sets-container").append(modalMarkup);

                windowBody.find(".form-control").not(".k-dropdown").css("height", "15px");
                windowBody.find(".col-md-10.col-lg-10").removeClass("col-md-10 col-lg-10").addClass("col-md-8 col-lg-8");
                if ($("body").hasClass("k-rtl")) {
                    windowBody.css({ "text-align": "right", "direction": "rtl" });
                    windowBody.find("[class*='col-']").css("float", "right");
                }
                windowBody.data("kendoWindow").center().open(); //show window
                windowBody.find("[name='Name']").val(newVal); //bind name

                windowBody.on('click', '#saveAddOption', saveOption);
            }

            function saveOption(e) {
                if (e) e.preventDefault();
                if (objectName != "undefined" || formData.IsLookUp == 2) {
                    formData._Name = $(windowBody).find("[name='Name']").val();
                    formData.ColumnsNames = [];
                    formData.ColumnsValue = [];
                    windowBody.find("input[name], select[name]").not(":button").each(function (index, node) {
                        //Model Values
                        if (node.name != "Id") {
                            formData.ColumnsNames.push(node.name);
                            if (node.type != "checkbox") formData.ColumnsValue.push(node.value);
                            else formData.ColumnsValue.push(node.checked);
                        }
                    });
                }

                $.post('/Pages/AddOption', { model: formData }, function (data) {
                    if (data == 2) {
                        formData.IsLookUp = 2; //user code
                        formData.SourceName = codeName;
                        $.get("/LookUpCode/ReadSysCodeId", { CodeName: codeName }, function (list) {
                            var data = []; //name, sysCode
                            var nameField = new FormJs.field({ name: "Name", type: "text", label: msg.Name, value: newVal, required: "required" });
                            var sysCodeField = new FormJs.field({ name: "SysCodeId", type: "select", label: msg.SysCodeId, required: "required" });
                            data = [nameField, sysCodeField];
                            DrawFormWindow(data);

                            for (var i = 0; i < list.length; i++) {
                                list[i].id = list[i].value;
                                list[i].name = list[i].text;
                            }
                            FormJs.fillOptionsDynamic(windowBody.find("form"), "SysCodeId", list, null, { hasFilter: false });
                        });
                    }
                    else 
                        FormJs.postSuccessFunc($(windowBody), e, data, function (savedData) {
                            windowBody.data("kendoWindow").close();

                            var dropDown = $("#" + name + " [name='" + input + "']").data("kendoDropDownList");
                            if (dropDown) {
                                dropDown.dataSource.add({ value: savedData._Id, text: savedData._Name });
                                dropDown.value(savedData._Id);
                            }
                            var grid = $('#' + name).data("kendoGrid");
                            var column = findInArray(grid.columns, input);
                            column.values.push({ value: savedData._Id, text: savedData._Name });

                            //Bind Added Item
                            var row = $("#" + name + " [data-uid=" + uid + "]");
                            if (!dropDown) {
                                var td = $(row).find("#" + name + "_active_cell");
                                td.text(savedData._Name);
                                currentTd = td;
                            }
                            var model = grid.dataItem(row);
                            model[input] = savedData._Id;
                            model.dirty = true;
                        });
                });
            } //end save function

            function castGridColumns(column) {
                column.name = (column.name == undefined ? column.ColumnName : column.name);
                column.order = column.ColumnOrder;
                column.Id = column.name;
                switch (column.ColumnType) {
                    case "number":
                        column.type = "number";
                        break;
                    case "boolean":
                        column.type = "checkbox";
                        break;
                    case "date":
                        column.type = "date"
                        break;
                    default:
                        column.type = "text";
                        break;
                }
                if (column.OrgInputType == "select" || column.InputType == "select")
                    column.type = "select";

                return column;
            }
        }
    }
  
    var deleteRecord = function (e) {
        e.preventDefault();
        e.stopPropagation();
        $(e.currentTarget).clearQueue();
        var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
        var grid = $(e.currentTarget).closest("[data-role=grid]");    
        bootbox.confirm(msg.AreYouSureToDelete.replace("{0}", dataItem[$(grid).attr("recordName")]),
            function (result) {
                if (dataItem.Id != 0) {
                    if (result) {
                        $("#LoadingDeleteImg").removeClass("hide");
                        $.ajax({
                            url: $(grid).attr("deleteUrl") + '/' + dataItem.Id,
                            type:"Post",
                            success: function (res) {
                                if (!res.Errors || res.Errors.length == 0) {
                                    toastr.success(dataItem[$(grid).attr("recordName")] + " " + msg.SuccessfullyDeleted);
                                    var dataSource = $('#' + $(grid).attr("id")).data("kendoGrid").dataSource;
                                    dataSource.remove(dataItem);
                                    var indexf = dataSource._pristineData.findIndex(a=>a.Id == dataItem.Id);
                                    dataSource._pristineData.splice(indexf,1);

                                } else {
                                    var message = "";
                                    if (res.Errors) {
                                        for (var i = 0; i < res.Errors.length; i++) {
                                            for (var k = 0; k < res.Errors[i].errors.length; k++) {
                                                message += res.Errors[i].errors[k].message;
                                            }
                                        }
                                    }
                                    toastr.error(message);
                                }
                                $("#LoadingDeleteImg").addClass("hide");
                            },
                            error: function () {

                                $("#LoadingDeleteImg").addClass("hide");
                                toastr.error(msg.DeleteRelatedInformation);

                            }
                        });
                    }
                } else
                {
                    var dataSource = $('#' + $(grid).attr("id")).data("kendoGrid").dataSource;
                    dataSource.remove(dataItem);
                }
                });

            }

    var editRecord = function (e) {
        e.preventDefault();
        var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
        updateHistory($(e.currentTarget).closest("[data-role=grid]").attr("editUrl") + '/' + dataItem.Id + '?Read=0&Version=' + $.urlParam('Version') + '&DataLevel=' + $.urlParam('DataLevel') + '&RoleId=' + $.urlParam('RoleId') + '&MenuId=' + $.urlParam('MenuId')+'&SSMenu='+$.urlParam('SSMenu'));
    }

    var showRecord = function (e) {
        e.preventDefault();
        var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
        updateHistory($(e.currentTarget).closest("[data-role=grid]").attr("editUrl") + '/' + dataItem.Id + '?Read=1&Version=' + $.urlParam('Version') + '&DataLevel=' + $.urlParam('DataLevel') + '&RoleId=' + $.urlParam('RoleId') + '&MenuId=' + $.urlParam('MenuId') + '&SSMenu=' + $.urlParam('SSMenu'));
    }

    function columnsHide(name) {
        return function (e) {
            e.preventDefault();
            var designChanged = $('#' + name).attr("designChanged");
            if (admin == "True" && designChanged == "false") changed(name, "Design", true);
            var grid = e.sender;
            var key = $('#' + name).attr("objectName");
            var db = localStorage[key + "-grid-options"];
            var newOptions = db ? JSON.parse(db) : {};
            var columns = grid.columns;
            var columnsHidden = newOptions["columnsHidden"] != undefined ? newOptions["columnsHidden"] : []; //old hidden if exists
            var x = columnsHidden.length;
            for (var i = 0; i < columns.length; i++) {
                if (columns[i].hidden) {
                    var obj = {};
                    if (columns[i].field) {
                        obj.field = columns[i].field
                    }
                    else {
                        obj.field = columns[i].id;
                    }

                    //if already exists -> don't add
                    var objFound = columnsHidden.filter(function (item) { return item.field == obj.field });
                    if (objFound.length == 0) {
                        columnsHidden[x] = obj;
                        x++
                    }
                }
                else {
                    columnsHidden = columnsHidden.filter(function (item) { return item.field != (columns[i].field ? columns[i].field : columns[i].id) });
                }
            }

            newOptions["columnsHidden"] = columnsHidden;

            if (admin != "True") 
                localStorage[key + "-grid-options"] = JSON.stringify(newOptions);     
            
        }
         
    }

    function redrawGrid(source, ref, gridtype) {
        var name;
        if (typeof (ref) == "string")
            name = ref;
        else
            name = $(ref).closest("[data-role=grid]").attr("id");

        var grid = $('#' + name).data("kendoGrid");
        var key = $('#' + name).attr("objectName");
        var dbOptions;
        var db = localStorage[key + "-grid-options"];
        
        if (db) {
            dbOptions = JSON.parse(db);
            grid.filterable = dbOptions.filterable || { mode: "menu" };
            grid.columnMenu = dbOptions.columnMenu;
        } else {
            var options = grid.getOptions();
            grid.filterable = options.filterable || { mode: "menu" };
            grid.columnMenu = options.columnMenu;
        }

        // Apply menu buttons click
        if (source == 1) {
            if (gridtype == 'batch') {
                $("#" + name + "f1").remove();
                $("#" + name + "c1").remove();
            }

            if (grid.filterable.mode == "menu")
                grid.filterable.mode = "menu,row";
            else
                grid.filterable.mode = "menu";

            grid.setOptions({
                filterable: { mode: grid.filterable.mode }
            });

        } else if (source == 2) {
            if (gridtype == 'batch') {
                $("#" + name + "f1").remove();
                $("#" + name + "c1").remove();
            }

            grid.columnMenu = !grid.columnMenu;
            grid.setOptions({
                columnMenu: grid.columnMenu
            });
        }

        // update local database key
        if (db) {
            dbOptions.filterable = grid.filterable || { mode: "menu" };
            dbOptions.columnMenu = grid.columnMenu;
            localStorage[key + "-grid-options"] = JSON.stringify(dbOptions);
        }

        // Add buttons to menu
        if (gridtype == 'batch') {
            if (grid.filterable.mode == "menu") {
                $('#' + name + " #toolsMenu").append("<li id='" + name + "f1'><a class='glyphicon glyphicon-share-alt' onclick='Grids.redrawGrid(1, this)'><span>&nbsp; " + msg.RowFilter + "</span></a></li>");
            } else {
                $('#' + name + " #toolsMenu").append("<li id='" + name + "f1'><a class='glyphicon glyphicon-remove' onclick='Grids.redrawGrid(1, this)'><span>&nbsp; " + msg.RowFilter + "</span></a></li>");
            }

            if (grid.columnMenu == true) {
                $('#' + name + " #toolsMenu").append("<li id='" + name + "c1'><a class='glyphicon glyphicon-remove' onclick='Grids.redrawGrid(2, this)'><span>&nbsp; " + msg.ColumnMenu + "</span></a></li>");
            } else {
                $('#' + name + " #toolsMenu").append("<li id='" + name + "c1'><a class='glyphicon glyphicon-share-alt' onclick='Grids.redrawGrid(2, this)'><span>&nbsp; " + msg.ColumnMenu + "</span></a></li>");
            }
        }

        //if (admin == "True") { // && gridName != "#ColumnProp"
        //   // setTimeout(function () { $('#' + name + ' th > a.k-link').click(); }, 1000);
        //    $('#' + name + ' th > a.k-link').dblclick(function () {
        //        $(this).attr('contenteditable', 'true');
        //        $(this).focus();
        //        $(this).addClass("k-input");
        //    });

        //    $('#' + name + ' a.k-link').focusout(function () {
        //        $(this).parent().attr("data-title", $(this).text());
        //        $(this).removeClass("k-input");
        //    });

        //    $('#' + name + ' th > a.k-link').each(function () {
        //        $(this).attr("data-toggle", "tooltip");
        //        $(this).attr("data-placement", "bottom");
        //        $(this).attr("title", msg.RenameColumn);
        //        $(this).tooltip();
        //    });
        //}


    }

    function resetGrid(name) {
        var objectName = $('#' + name).attr("objectName");
        var version = +$.urlParam('Version');
        
        localStorage.removeItem($('#' + name).attr("objectName") + "-grid-options");

        if (admin == "True") {
            $.ajax({
                url: "../../Pages/resetGrid",
                data: { ObjectName: objectName, Version: version },
                dataType: 'json',
                contentType: 'application/json',
                success: function (result) {
                    if (result == "OK") {
                        toastr.success(msg.SaveDesignSuccessfully);
                       // $("#renderbody").load(ulr);
                        updateHistory(ulr);
                    }
                }
            });
        } else {
           // $("#renderbody").load(ulr);
            updateHistory(ulr);
        }
        //  location.reload();
    }

    function saveAsPdf(name) {
        var grid = $('#' + name).data("kendoGrid");
        grid.saveAsPdf();

        //$("#" + name).find(".k-grid-toolbar").css("display", "none");
        //$("#" + name).find(".k-grouping-header").css("display", "none");
        //kendo.drawing.drawDOM("#" + name, {
        //    avoidLinks: true,
        //    multiPage: true,
        //    margin: "2cm",
        //    pageSize: "A4"
        //}).then(function (group) {
        //    kendo.drawing.pdf.saveAs(group, "ExportPdf.pdf");
        //    $("#" +name).find(".k-grid-toolbar").css("display", "");
        //    $("#" + name).find(".k-grouping-header").css("display", "");
        //});
    }
    function ExportChart(name) {
        var chart = $("#" + name).data("kendoChart");
        if (chart.dataSource._total != 0) {
            chart.exportImage().done(function (data) {
                kendo.saveAs({
                    dataURI: data,
                    fileName: "chart.png"
                });
            });
        } else
            toastr.error(msg.NoDataFound);
    }
    function saveAsXsl(name) {
        var grid = $('#' + name).data("kendoGrid");
        grid.saveAsExcel();
    }
    function ExportDiagram(name) {
        var Diagram = $("#" + name).data("kendoDiagram");
        var rtl = $("body").css("direction");
        if (rtl == "rtl")
            $("body").css("direction","ltr");
        if (Diagram.dataSource._total != 0) {
            Diagram.exportImage().done(function (data) {
                kendo.saveAs({
                    dataURI: data,
                    fileName: "Diagram.png"
                });
                if (rtl == "rtl")
                    $("body").css("direction", "rtl");
            });
        } else
            toastr.error(msg.NoDataFound);
    }
    function saveGrid(name) {
        if (admin != "True") return;
        var grid = $('#' + name).data("kendoGrid");
        var options = {};
        var columns = grid.columns;
        var titles = [];
        var roles = [];
        var columninfo = [];
        var x = 0, y = 0, z = 0;
        var objectName = $('#' + name).attr("objectName");

        //var db = localStorage[name + "-IColumns"]
        //var roleColumns = db && JSON.parse(db);

        for (var i = 0; i < columns.length; i++) {

            var info = {};
            info.ObjectName = objectName;
            info.ColumnName = columns[i].field || columns[i].id;
            info.ColumnOrder = i + 1;
            info.DefaultWidth = columns[i].width;
            if (columns[i].hidden)
                info.isVisible = false;
            else
                info.isVisible = true;
            columninfo[x] = info;
            x++;

            if (columns[i].field) {
                var title = {};
                title.ObjectName = objectName;
                title.Culture = lang;
                title.ColumnName = columns[i].field;
                title.Title = $('#' + name + " thead th[data-field='" + columns[i].field + "']").attr("data-title");
                if (title.Title == undefined) title.Title = columns[i].field;
                titles[y] = title;
                y++;
            }

            //if (roleColumns && roleColumns[i]) {
            //    for (var k = 0; k < roleColumns[i].roles.length; k++) {
            //        if (roleColumns[i].roles[k]) {
            //            var role = {};
            //            role.RoleId = roleColumns[i].roles[k];
            //            role.ObjectName = objectName;
            //            role.ColumnName = roleColumns[i].column;
            //            role.isVisble = false;
            //            roles[z] = role;
            //            z++;
            //        }
            //    }
            //}
        }

        options.ObjectName = objectName;
        options.TableName = $('#' + name).attr("tableName");
        options.Version = $.urlParam('Version');
        options.MenuId = $.urlParam('MenuId');
        options.Lang = lang;
        options.columnTitles = titles;
        //options.roleColumns = roles;
        options.ColumnInfo = columninfo;

        $.ajax({
            url: "../../Pages/SaveGrid",
            type: 'POST',
            data: JSON.stringify(options),
            dataType: 'json',
            contentType: 'application/json',
            success: function (result) {
                if (result == "OK") {
                    toastr.success(msg.SaveDesignSuccessfully);
                    var designChanged = $('#' + name).attr("designChanged");
                    if (designChanged == "true") changed(name, "Design", false);
                }
                else
                    toastr.error(result);
            },
            error: function (result) {
                toastr.error(result.responseText);
            }
        }
      );

    }

    function deleteAllEmp(name) {
        bootbox.confirm(msg.AreYouSureToDelete.replace("{0}",name),
           function (result) {
               if (result) {
                   $.ajax({
                       url: "../../People/DelateAllEmployees",
                       dataType: 'json',
                       contentType: 'application/json',
                       success: function (res) {
                           if (res == "OK") {
                               toastr.success(msg.SuccessfullyDeleted);
                               $("#AssignmentDiv").data('kendoGrid').dataSource.read();
                               $("#AssignmentDiv").data('kendoGrid').refresh();
                           }else
                               toastr.error(res);
                       }
                   });
               }
           });
    }
    return {
        indexGrid: indexGrid,
        batchGrid: batchGrid,
        deleteClick: deleteClick,
        addNewClick: addNewClick,
        saveClick: saveClick,
        cancelClick: cancelClick,
        customEditor: customEditor,
        deleteRecord: deleteRecord,
        editRecord: editRecord,
        redrawGrid: redrawGrid,
        resetGrid: resetGrid,
        saveGrid: saveGrid,
        sendData: sendData,
        saveAsPdf: saveAsPdf,
        saveAsXsl: saveAsXsl,
        deleteAllEmp :deleteAllEmp,
        ExportChart: ExportChart,
        ExportDiagram: ExportDiagram,
        addOption: addOption,
        setErrors: setErrors,
        toValidJson: toValidJson,
        TriggerChanges:changed,
        CheckBoxChanged: CheckBoxChanged
    }
}();