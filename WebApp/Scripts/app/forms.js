// OmegaForm.JS v 1.0 library depends on  JQUERY , JQUERY UI 
//can be extended by other thirdparty templates , plugins
//-----------------------------------------------------
/* a small framework which dynamically generates fields
with (bootstrap , JQuery , Kendo UI ) 
The generator constructor accepts JSON passed by developers  
Generated Form fields are recordable; labels are editable by the app admin  */
//------------------------------------------------------
// started in 14-7-2016
// last updated 28-8-2016 5:00pm by H1outlook@outlook.com - *E.Hamada Mahmoud* for 'Omega group - HR Team'
// 18-8 idea   : 
// OmegaForm shouldn't depend on any more javascript library for drawing custom UI elements 
// other than opensource , free , stable and popular  (bootstrap) but instead it should by extendable to work 
// 

var FormJs = function () {
    "use strict";

    // #region global prototypes , Enums , functions , variables
    var layouts = {
        basic: "",
        inline: "form-inline",
        horizontal: "form-horizontal"
    }

    var FieldSizes = {
        Large: "form-group-lg",
        Small: "form-group-sm",
        Normal: ""
    }

    var FormMethods = {
        POST: "post",
        GET: "get"
    }

    var Panels = {
        valid: "panel-success",
        invalid: "panel-danger",
        haschanges: "panel-info",
        warnning: "panel-warning",
        design: "panel-primary",
        start: "panel-default"
    }

    var formIcons = {
        startedit: "glyphicon glyphicon-pencil",
        haschanges: "glyphicon glyphicon-asterisk",
        invalid: "glyphicon glyphicon-remove-sign",
        valid: "glyphicon glyphicon-ok-sign",
        warnning: "glyphicon glyphicon-exclamation-sign",
        design: "glyphicon glyphicon-cog"
    }

    var FieldTypes = {
        Email: "email",
        Label: "label",
        Password: "password",
        AutoComplete: "autocomplete",
        MaskedTextBox: "maskedtextbox",
        File: "file",
        Checkbox: "checkbox",
        DropDownList: "select",
        MultiSelect: "multiselect",
        MultiLineText: "textarea",
        radioset: "radioset",
        Radio: "radio",
        Url: "url",
        percent: "percent",
        Text: "text",
        HtmlColor: "color",
        Date: "date",
        DateTime: "datetime",
        HtmlRange: "range",
        Time: "time",
        HtmlSearch: "search",
        HtmlTelephone: "tel",
        Button: "button"
    }

    var Orintations = {
        vertical: "",
        horizontal: "-inline"
    }

    function fieldset(options) {
        /// <field type='Boolean'>The.</field>
        this.Id = options.Id;
        this.SetId = options.SetId;
        // default values of the following properties will be inherited from the OmegaForm object
        this.layout = options.layout;
        this.Reorderable = options.Reorderable;
        this.LabelEditable = options.LabelEditable;
        this.Collapsable = options.Collapsable;

        this.HasFieldSetTag = options.HasFieldSetTag === undefined ? true : options.HasFieldSetTag; // false should cause the renderer to render div instead of fieldset
        this.legend = options.legend;
        this.legendTitle = (options.legendTitle ? options.legendTitle : options.legend);

        this.Sections = options.Sections === undefined ? [] : options.Sections;
        this.Freez = options.Freez === undefined ? false : options.Freez;
        this.order = options.order;

        this.Collapsed = options.Collapsed === undefined || this.Collapsable == false ? false : options.Collapsed;
    }

    function section(options) {
        this.Id = options.Id;
        this.SectionTitle = options.SectionTitle;
        this.name = options.name;
        this.order = options.order
        this.layout = options.layout;

        //-------------
        this.fieldsNumber = options.fieldsNumber; //for inline layout
        this.Reorderable = options.Reorderable === undefined ? true : options.Reorderable;

        this.labelsm = options.labelsm == undefined ? 2 : options.labelsm; // by default labels will take 2 columns in horizintal & inline layout
        this.labelmd = options.labelmd == undefined ? this.labelsm : options.labelmd; // by default md inherites sm
        this.labellg = options.labellg == undefined ? this.labelmd : options.labellg; // ny default lg inherties md
        this.Freez = options.Freez;

        this.fields = options.fields;
    }

    function field(options) {

        for (var i in options) {
            this[i] = options[i];
        }

        this.name = options.name;
        this.id = options.id;
        if (this.name != undefined && (options.id == undefined || !isNaN(options.id))) {
            this.id = this.name;
        }
        this.order = options.order;
        this.type = options.type != undefined ? options.type : "text";
        this.size = options.size === undefined ? FieldSizes.Normal : options.size;
        this.sm = (options.sm == undefined) ? 12 : options.sm;

        this.md = options.md == undefined ? this.sm : options.md;
        this.lg = options.lg == undefined ? this.md : options.lg;
        this.value = options.value === undefined ? "" : options.value;

        this.label = options.label == undefined ? options.name : options.label;

        this.SrOnly = options.SrOnly === undefined ? false : options.SrOnly;
        //this.placeholder = options.placeholder === undefined && this.type != "radio" && ? this.label : options.placeholder;

        //initial list with a single empty option
        this.list = options.list;
        this.codeName = options.CodeName;
        this.DefaultValue = options.DefaultValue == "" ? undefined : options.DefaultValue;
        this.value = options.DefaultValue;

        this.formula = options.Formula;

        this.isVisible = options.isVisible;
        this.class = options.class === undefined ? "" : options.class;
        this.orintation = options.orintation === undefined ? Orintations.horizontal : options.orintation;
    }

    function listitem(options) {
        this.id = options.id === undefined ? "" : options.id;
        this.name = options.name === undefined ? "" : options.name;
        this.disabled = options.disabled === undefined ? false : options.disabled;
        this.selected = options.selected === undefined ? false : options.selected;
    }

    function fillOptionsDynamic(context, elementName, optionList, model, options) {

        //options: { remoteTableName, objectName, hasFilter }

        //Add List Dynamic from ViewBag -- model(optional): for model binding.
        var optionsMarkup = "", myField = $(context).find("#" + elementName);

        if (model && model[elementName] != undefined && typeof (model[elementName]) == "string") model[elementName] = model[elementName].trim(); //for problems like language

        //Show mode
        if ($(context).attr("mode") == "show" && myField.attr("show-type") == undefined) {
            if (model && model[elementName] != undefined) {
                var value = "";
                if (optionList) {
                    for (var i = 0; i < optionList.length; i++) {
                        if (Array.isArray(model[elementName])) {
                            for (var j = 0; j < model[elementName].length; j++) {
                                if (optionList[i].id == model[elementName][j]) value += optionList[i].name + ", ";
                            }
                        } else
                            if (optionList[i].id == model[elementName]) value = optionList[i].name;
                    }

                    $("#" + elementName).text(value);
                } else if (options && options.remoteTableName && model[elementName] != undefined) {
                    var url = '/Pages/ReadRemoteList'; //?tableName=' + options.remoteTableName + '&Id=' + model[elementName];
                    $.get(url, { tableName: options.remoteTableName, query: null, Id: model[elementName] }, function (res) {
                        if (res && res.length) $("#" + elementName).text(res[0].name);
                    });
                }
            }
        }
        else {
            //isActive -> to bind data if "end effect date" < Today and object is used 
            var activeOptionList = [];
            if (optionList && $.isArray(optionList)) {
                activeOptionList = optionList.filter(function (element) { return (element.isActive == true || element.isActive == undefined) });
                if (optionList && model && model[elementName] != undefined && $.inArray(model[elementName], activeOptionList) == -1) {
                    if (Array.isArray(model[elementName])) {
                        for (var j = 0; j < model[elementName].length; j++) { //multi select
                            var obj = optionList.filter(function (element) { return (element.id == model[elementName][j] && element.isActive == false) });
                            if (obj.length) activeOptionList = activeOptionList.concat(obj);
                        }
                    } else {
                        //var obj = optionList.find(e => e.id == model[elementName] && e.isActive == false);
                        var obj = optionList.filter(function (e) { return e.id == model[elementName] && e.isActive == false; });
                        if (obj && obj.length) activeOptionList.push(obj[0]);
                    }
                }
            }

            //Select or AutoComplete
            if (myField.length) {
                //Select
                var hasPic = myField.attr('has-pic') == 'true', hasIcon = myField.attr('has-icon') == 'true', companyid = $(context).attr("companyid");

                if (myField[0].localName == "select") {

                    if (options && options.objectName) $(myField[0]).attr("objectname", options.objectName);

                    if (myField.prop("multiple") == false) { //Select
                        var isFreez = activeOptionList.length && activeOptionList[0].isFreez == true; //if lookup is protected
                        var isAllowed = !isFreez && $(myField[0]).attr("allowed") == "true" && (options && options.objectName || $(myField[0]).attr("codename"));

                        //template
                        var noData = "<div> " + allMsgs.NoDataFound + (isAllowed ? " <input type='button' class='k-button addOption addOptionF' value='" + allMsgs.AddNewOption + "' formId='" + $(context).attr("id") + "' elementName='" + elementName + "' />" : "") + "</div>";
                        var template = "<div  class='myCustTemp'>" + (hasPic ? "<img class='k-people-photo' src=#:Exist(data.Gender,data.PicUrl)# />&nbsp;" : "") +
                            (hasIcon ? "<i class='ace-icon fa fa-circle stat#:data.Icon#'></i>&nbsp;" : "") +
                            "<div class='k-people-name'>#: data.name # </div></div>";

                        var hasFilter = true;
                        if (options) hasFilter = options.hasFilter != false
                        else if ($(myField[0]).attr("has-filter")) hasFilter = ($(myField[0]).attr("has-filter") != "false");
                        $(myField).kendoDropDownList({
                            valuePrimitive: true,
                            dataSource: activeOptionList,
                            dataTextField: "name",
                            dataValueField: "id",
                            template: (hasIcon || hasPic ? template : ""),
                            valueTemplate: (hasIcon || hasPic ? template : ""),
                            value: (model && model[elementName] != undefined ? model[elementName] : ""),
                            optionLabel: " ",
                            filter: !hasFilter ? "none" : "contains",
                            noDataTemplate: noData
                        });
                    }
                    else { //Multi Select
                        myField.kendoMultiSelect({
                            valuePrimitive: true,
                            filter: "contains",
                            dataSource: activeOptionList,
                            dataTextField: "name",
                            filter: "contains",
                            dataValueField: "id",
                            value: (model && model[elementName] != undefined ? model[elementName] : []),
                            animation: {
                                close: {
                                    effects: "zoom:out",
                                    duration: 300
                                }
                            }
                        }).data("kendoMultiSelect");
                    }
                }
                else if ($(myField).attr("type") == "autocomplete") {
                    //AutoComplete
                    var isFirst = true, isValid = true, isSelect = false;
                    $(myField).val("");
                    var optionSource;
                    if (options && options.remoteTableName) {
                        $(myField).attr('tablename', options.remoteTableName);
                        var remoteUrl = '/Pages/ReadRemoteList?tableName=' + options.remoteTableName + '&formTblName=' + $(context).attr("tablename") + '&query=%QUERY';
                        optionSource = new Bloodhound({
                            datumTokenizer: Bloodhound.tokenizers.obj.whitespace('name'),
                            queryTokenizer: Bloodhound.tokenizers.whitespace,
                            //prefetch: '/Pages/ReadRemoteList?tableName=' + options.remoteTableName, //cache
                            remote: {
                                url: remoteUrl + (!model || model.Id != 0 || model[elementName] != undefined ? '&Id=' + model[elementName] : ''),
                                wildcard: '%QUERY',
                                filter: function (lst) {
                                    if (isFirst) {
                                        if (model && model.Id != 0 && lst.length) {
                                            var obj = lst.find(o => o.id == model[elementName]);
                                            if (obj) {
                                                myField.prop("data-val", obj.id);
                                                myField.typeahead('val', obj.name);
                                            }
                                            myField.blur();
                                        }
                                        isFirst = false;
                                        optionSource.remote.url = remoteUrl;
                                    } else {
                                        isFirst = false;
                                        return lst;
                                    }
                                }
                            }
                        });
                    } else {
                        optionSource = new Bloodhound({
                            datumTokenizer: Bloodhound.tokenizers.obj.whitespace('name'),
                            queryTokenizer: Bloodhound.tokenizers.whitespace,
                            local: activeOptionList
                        });
                    }

                    myField.typeahead({
                        minLength: 0,
                        highlight: true
                    },
                    {
                        name: elementName,
                        display: 'name',
                        source: optionSource,
                        templates: {
                            suggestion: function (data) {
                                if (data) if (hasPic)
                                        return "<div class='myCustTemp'>" + (hasPic ? "<img class='k-people-photo' src=" + Exist(data.Gender, data.PicUrl) + ">&nbsp;" : "") +
                                            (hasIcon ? "<i class='ace-icon fa fa-circle stat" + data.Icon + "'></i>&nbsp;" : "") +
                                            "<div class='k-people-name'>" + data.name + "</div></div>"; else return "<div class='myCustTemp'>" + data.name + "</div>";
                            }
                        }
                    }).on("typeahead:select", function (e, item) {
                        isSelect = true;
                        myField.prop("data-val", item.id);
                        isValid = true;
                        if (myField.hasClass("k-invalid")) {
                            myField.remove("k-invalid").prop("aria-invalid", false);
                            myField.next().remove();
                        }
                    }).on('select', function () {
                        if (isFirst) setTimeout(function () { myField.focus(); }, 100);
                    }).on('change', function (e) {
                        if (!isSelect) isValid = false;
                    }).on('blur', function (e) {
                        isSelect = false;
                        if (!myField.hasClass("k-invalid") && !isValid && myField.val() != "") {
                            myField.removeClass("k-valid");
                            myField.addClass("k-invalid").prop("aria-invalid", true);
                            myField.after("<span class='k-widget k-tooltip k-tooltip-validation k-invalid-msg' data-for='" + elementName + "' role='alert'>" + allMsgs.notValidValue + "</span>");
                        }
                    });
                    //To display the name not id
                    if (model) {
                        if (!options || !options.remoteTableName) {
                            for (var i in activeOptionList) {
                                if (activeOptionList[i].id == model[elementName]) {
                                    myField.prop("data-val", activeOptionList[i].id);
                                    myField.typeahead('val', activeOptionList[i].name);
                                    break;
                                }
                            }
                        } else {
                            myField = $("[name='" + elementName + "']");
                            if (options && options.remoteTableName == 'World') isFirst = false;
                            else if (isFirst && model.Id != 0 && model[elementName] != undefined) myField.select();
                            else if (model.Id == 0 || model[elementName] == undefined) isFirst = false;
                            optionSource.remote.url = remoteUrl;
                        }
                    }
                }
            }
            else {
                //Radioset
                for (var i in activeOptionList) {
                    var option = new listitem(activeOptionList[i])
                    optionsMarkup += "<label class='radio-inline " + (option.disabled ? " disabled" : "") + "'>" + "<input type='radio'" + (option.selected || model && model[elementName] == option.id ? " checked" : "") + (option.disabled ? " disabled" : "") + " name='" + elementName + "' id='" + elementName + i + "' value='" + option.id + "' /> " + option.name + "</label>";
                }
                $("[for=" + elementName + "]").next("div").append(optionsMarkup);
            }
        }
        //OldValues
        if (model) {
            var oldValue = {};
            oldValue.ColumnName = ((Array.isArray(model[elementName]) && elementName.indexOf('I') == 0) ? elementName.substr(1) : elementName);

            oldValue.Text = "";
            if (model[elementName] != undefined) {
                var value = "";
                if (optionList) {
                    for (var i = 0; i < optionList.length; i++) {
                        if (Array.isArray(model[elementName])) {
                            for (var j = 0; j < model[elementName].length; j++) { //multi select
                                if (optionList[i].id == model[elementName][j]) value += optionList[i].name + ", ";
                            }
                        } else
                            if (optionList[i].id == model[elementName]) value = optionList[i].name;

                    }
                }
                oldValue.Text = value;
            }
            var formId = $(context).attr("id");
            if (oldModel[formId]) oldModel[formId].push(oldValue);
        }
    }

    var oldModel = {}, allMsgs = {};
    // #endregion

    // #region OmegaForm jquery extention
    jQuery.fn.extend({

        Forms: function (options, model) {
            ///<summary>dynamically generates fields . The generator constructor accepts JSON passed by developers .. Generated Form fields are recordable; labels are editable by the app admin</summary>
            ///<returns type=""></returns>
            // #region internal properties : a code to set the Form properties
            var hasdatachanges = false;
            var hasdesignchanges = false;
            var IntializedForSort = false;
            //var IntializedForEdit = false;
            var rtl = options.rtl === undefined ? false : options.rtl;
            var ExpandClass = options.ExpandClass == undefined ? "" : options.ExpandClass.trim();
            var CollabseClass = options.CollabseClass == undefined ? "" : options.CollabseClass.trim();
            var admin = options.admin;
            var formTitle = options.Title;

            var modelId;
            if (model != undefined) modelId = model.Id;
            else model = {};

            var HasFormTag = options.HasFormTag === undefined ? true : options.HasFormTag;

            if (HasFormTag) {
                var FormMethod = options.FormMethod === undefined ? FormMethods.POST : options.FormMethod;
                if (!isvalid(FormMethods, FormMethod)) {
                    throw new Error("OmegaForm Error : code 1 : The form method is not valid .. set it to avalid (get or Post) or use the FormMethods Enum");
                    //debugger;
                }
                var FormAction = options.FormAction === undefined ? "/" : options.FormAction;
                var FormId = options.FormId;
            }

            var HasPanel = options.HasPanel === undefined ? true : options.HasPanel;
            var HasEditControls = options.HasEditControls === undefined ? true : options.HasEditControls;
            var mode = options.mode;

            if (mode == "show" || admin != 'True') HasEditControls = false;

            var reorderable = options.reorderable === undefined ? true : options.reorderable;
            var labelEditable = options.labelEditable === undefined ? true : options.labelEditable;
            var layout = options.layout === undefined ? layouts.horizontal : options.layout;

            var tableName = options.TableName;
            var objectName = options.ObjectName;
            var version = options.Version == undefined ? 0 : options.Version;
            var culture = options.Culture;
            var SessionVars = options.SessionVars == undefined ? [] : JSON.parse(options.SessionVars);

            var JsMessages = ((options.JsMessages == "empty" || options.JsMessages == undefined) ? [] : JSON.parse(options.JsMessages));
            if (JsMessages != undefined) {
                for (var i = 0; i < JsMessages.length; i++) {
                    allMsgs[JsMessages[i].name] = JsMessages[i].msg;
                }
            }

            if (HasPanel) {
                var PanelType = options.PanelType === undefined ? Panels.start : options.PanelType;
                var Title = options.Title === undefined ? "New form ..." : options.Title;
                var Icon = options.Iconclass === undefined ? formIcons.startedit : options.Iconclass;
                var originalIcon = Icon;
                var originalpanel = PanelType;
                var PanelId = options.PanelId;
            }

            var lookupCodes = (options.CodesLists == undefined ? [] : options.CodesLists);
            var allowInsetCode = options.AllowInsert;
            var formBtns = (options.btns ? options.btns : []);
            var formreqBtns = (options.reqbtns ? options.reqbtns : []);
            var formulaSelectors = [], formulaObjArr = []; //[{column: null, formula: null}]
            // #endregion

            // #region internal functions
            function isvalid(enumerator, value) {
                for (var i in enumerator) {

                    if (enumerator[i] == value.toLowerCase()) {
                        return true
                    }
                }
                return false;
            }

            // no accordion feature .. because there's a bug in bootstrap 3 that accordion depends on the dom structure .. depends on a panel 
            // https://github.com/twbs/bootstrap/issues/10966 .. H1 28 -8 :)
            // only expanding or collabsing the target element 
            function CollabseExpand(TrigerElement, TargetElement, Indecator, CollabseClass, ExpandClass) {
                ///<summary> collabses the ............ </summary>
                ///<param name="TrigerElement" type="jquery"> </param>
                ///<param name="TargetElement" type="jquery"> </param>
                ///<param name="Indecator" type="jquery"> </param>
                ///<param name="CollabseClass" type="string"> </param>
                ///<param name="ExpandClass" type="string"> </param>
                if (!$(TargetElement).hasClass('collapse')) {
                    $(TargetElement).addClass('collapse');
                }

                $(TargetElement).collapse('toggle');

                if (Indecator != undefined) {
                    if ($(Indecator).hasClass(CollabseClass)) {
                        $(Indecator).removeClass(CollabseClass).addClass(ExpandClass)
                    }
                    else if ($(Indecator).hasClass(ExpandClass)) {
                        $(Indecator).removeClass(ExpandClass).addClass(CollabseClass)
                    }
                }
            }

            function reorder(a, b) {
                ///general Reorder
                if (a.order === undefined) {
                    return 1;
                }
                if (b.order === undefined) {
                    return -1;
                }
                return (a.order - b.order);
            }

            // an internal function that generates the html of a field according to it's type

            // #region code for rendering  any html field according to passed params
            oldModel[FormId] = [];

            //array of none global htmlattributes field properties
            var NotHtmlAttributes = ["label", "sm", "md", "xs", "data_type", "type", "list", "inline", "lg", "SrOnly", "orintation", "section", "fieldset", "order", "autoComplete", "dataSourse"];

            function A3(f) {
                var myfield = new field(f);
                //iterate through all the field properties
                var additionalMarkup = "";
                if (options.DisabledColumns && options.DisabledColumns.indexOf(myfield.name) > -1) {
                    additionalMarkup += " disabled=true ";
                    if ((!model || model.Id == 0) && myfield.DefaultValue != undefined) {
                        additionalMarkup += " value='" + myfield.DefaultValue + "' ";
                        model[myfield.name] = myfield.DefaultValue;
                    }
                }

                if ((myfield.type == "date" || myfield.type == "time") && model && model[myfield.name] != undefined)
                    if (model[myfield.name].indexOf('/Date') != -1) {
                        if (mode == "show") {
                            if (myfield.type == "time") { var x = new Date(parseInt(model[myfield.name].substr(6)));  model[myfield.name] = kendo.toString(x, "t") }
                            else if (new Date(parseInt(model[myfield.name].substr(6))).getHours() == 0) model[myfield.name] = kendo.toString(new Date(parseInt(model[myfield.name].substr(6))), "d");
                            else model[myfield.name] = kendo.toString(new Date(parseInt(model[myfield.name].substr(6))), 'g');
                        } else if (myfield.type == "date") {
                            model[myfield.name] = parseServerDate(model[myfield.name]);
                        }
                        else if (myfield.type == "time") { var x = new Date(parseInt(model[myfield.name].substr(6)));  model[myfield.name] = kendo.toString(x, "HH:mm") }

                    }
                if (myfield.type != "checkbox" && myfield.type != "radio" && myfield.type != "radioset" && myfield.type != "label" && mode != "show") {
                    myfield.class += "form-control";
                    if (myfield.type != FieldTypes.DropDownList && myfield.type != "autocomplete") {
                        //Bind Data
                        if (model && model[myfield.name] != undefined)
                            additionalMarkup += " value='" + model[myfield.name] + "' ";
                    }
                }

                if (myfield.type == "checkbox" && model[myfield.name] == true || model[myfield.name] == 'true')
                    additionalMarkup += "checked";

                for (var prop in myfield) {
                    //skip the none html attributes properties and empty attributes
                    if (prop == "required" && myfield[prop])
                        additionalMarkup += " formreq='formreq'";
                    else if (prop == "minLength" && myfield[prop])
                        additionalMarkup += " formlength=" + myfield[prop];
                    else if (myfield[prop] && NotHtmlAttributes.indexOf(prop) == -1) {

                        if (prop == "pattern" && myfield[prop] == null) {
                        } else if (myfield[prop] == myfield.HtmlAttribute)
                            additionalMarkup += " " + myfield.HtmlAttribute;
                        else
                            additionalMarkup += " " + prop + "='" + myfield[prop] + "'";
                    }
                }

                //add the datalist id
                if (myfield.list != undefined && myfield.type != FieldTypes.DropDownList && myfield.type != FieldTypes.AutoComplete) {
                    additionalMarkup += " list ='" + myfield.name + "-list' ";
                }

                //Get columns names from formula
                if (myfield.formula) additionalMarkup += getFormulaColumns(myfield.name, myfield.formula);

                return additionalMarkup;
            }

            //get helpers columns for formula
            function getFormulaColumns(fieldName, formula) {
                var operators = /(=)|(>=)|(<=)|(<)|(>)|(\+)|(-)|(\*)|(\\)|(%)/,
                    columnsArr = formula.split(operators), helpersCols = [];

                for (var i = 0; i < columnsArr.length; i++) {
                    var item = columnsArr[i];

                    //skip null or operator or NaN 
                    if (item == undefined || !isNaN(Number(item)) || operators.test(item)) continue;

                    item = item.trim();
                    //skip if column not in model
                    if (!(model && model.hasOwnProperty(item))) continue;

                    formula = formula.replace(item, "model[\'" + item + "\']");  //model['Salary'] * 0.5
                    helpersCols.push("#" + item); //['#Salary']
                }

                var columns = helpersCols.join(','); // '#Salary'
                if ($.inArray(columns, formulaSelectors) == -1) formulaSelectors.push(columns);
                formulaObjArr.push({ column: fieldName, formula: formula });
                return " formula-columns = '" + columns + "' ";
            }

            function getinput(f) {
                f.type = f.type.toLowerCase();
                var myfield = new field(f);
                var datalistmarkup = "";
                //renders a datalist if required
                if (myfield.list != undefined && myfield.type != FieldTypes.MultiSelect && myfield.type != FieldTypes.DropDownList && myfield.type != FieldTypes.radioset && myfield.type != FieldTypes.AutoComplete) {
                    datalistmarkup = "<datalist id='" + myfield.name + "-list' >";
                    for (var i in myfield.list) {
                        var option = new listitem(myfield.list[i]);
                        datalistmarkup += "<option " + (option.disabled ? " disabled" : "");
                        if (option.selected) { additionalMarkup += " selected"; }
                        datalistmarkup += " value='" + option.id + "' > " + option.name + "</option>";
                    }
                    datalistmarkup += "</datalist>";
                }

                ///---Show Mode
                if (mode == "show" && (!myfield.HtmlAttribute || myfield.HtmlAttribute.indexOf("show-type") == -1)) {
                    if (myfield.type != 'checkbox' && myfield.type != 'button' && myfield.type != 'hidden') {
                        var additionalMarkup = '<label ' + A3(myfield) + '>';
                        if (model && model[myfield.name] != undefined)
                            if (myfield.type != 'radioset' && myfield.type != 'select' && myfield.type != 'autocomplete' && myfield.type != 'multiselect') {
                                additionalMarkup += model[myfield.name];
                            } else {
                                if (myfield.codeName) myfield.list = getLookUpCodes(myfield.codeName);
                                if (myfield.list) {
                                    for (var i = 0; i < myfield.list.length; i++) {
                                        if (myfield.list[i].id == model[myfield.name]) {
                                            additionalMarkup += myfield.list[i].name;
                                        }
                                    }
                                }
                            }
                        additionalMarkup += '</label>';
                    }
                    else if (myfield.type == 'button' && myfield.HtmlAttribute.indexOf("back") != -1) {
                        var additionalMarkup = '<input type="' + myfield.type + '" ' + A3(myfield) + ' />';
                    } else if (myfield.type == 'xtextarea') {
                        var additionalMarkup = '<textarea value="' + model[myfield.name] + '" readonly="" style="border: none; max-width: 1313px;"/>';
                    } else {
                        myfield.disabled = true;
                        var additionalMarkup = '<input type="' + myfield.type + '" ' + A3(myfield) + ' />';
                    }
                }
                else {
                    ///---Edit Mode
                    // add all html attributes to a field markup--- with minimum code :)
                    switch (myfield.type) {
                        case "textarea":
                            var additionalMarkup = "<textarea " + A3(myfield) + ">"
                            + (model != undefined && model[myfield.name] != undefined ? model[myfield.name] : "")
                            + "</textarea>";
                            break;
                        case "select":
                            if (myfield.codeName) myfield.list = getLookUpCodes(myfield.codeName);
                            var additionalMarkup = "<select allowed=" + allowInsetCode + " " + (myfield.list && myfield.list.length && myfield.list[0].isUserCode ? "usercode='true' " : "") + A3(myfield) + ">" + "</select>";
                            break;
                        case "multiselect":
                            if (myfield.codeName) myfield.list = getLookUpCodes(myfield.codeName);
                            var additionalMarkup = "<select multiple='multiple' " + A3(myfield) + ">";
                            var oldValues = {};
                            oldValues.ColumnName = myfield.name;
                            oldValues.Text = "";
                            for (var i in myfield.list) {
                                var option = new listitem(myfield.list[i]);
                                additionalMarkup += "<option " + (option.disabled ? " disabled" : "");
                                if (option.selected || (model != undefined && model[myfield.name] == option.id)) {
                                    oldValues.Text += option.name + ", ";
                                    additionalMarkup += " selected";
                                } //Bind data 
                                additionalMarkup += " value='" + option.id + "' > " + option.name + "</option>";
                            }
                            additionalMarkup += "</select>";
                            break;
                        case "hidden":
                            if (model) {
                                if (model.Id == 0 && myfield.value != undefined) myfield.value = myfield.value;
                                else if (model[myfield.name] != undefined) myfield.value = model[myfield.name];
                            }
                            var additionalMarkup = "<input type='hidden' id='" + myfield.name + "' name='" + myfield.name + "' "
                             + (myfield.value != null ? "value='" + myfield.value : "") + "' />";
                            break;
                        case "radioset":
                            if (myfield.codeName) myfield.list = getLookUpCodes(myfield.codeName);
                            var additionalMarkup = "";
                            for (var i in myfield.list) {
                                var option = new listitem(myfield.list[i])
                                additionalMarkup += "<label class='radio" + myfield.orintation + (option.disabled ? " disabled" : "") + "'>"
                                + "<input type='radio'";
                                if (model) {
                                    if (option.selected || (model[myfield.name] == option.id) || (myfield.DefaultValue == option.id)) {
                                        oldModel[FormId].push({ ColumnName: myfield.name, Text: option.name });
                                        additionalMarkup += " checked";
                                    }
                                }
                                additionalMarkup += (option.disabled ? " disabled" : "")
                                + " name='" + myfield.name + "' id='" + myfield.name + i + "' value='" + option.id + "' " + A3(myfield) + " /> " + option.name + "</label>";
                            }
                            break;
                        case "email":
                            var additionalMarkup = '<div class="input-group">'
                            + (!rtl ? "<span class='input-group-addon'>@</span><input type='" + myfield.type + "' " + A3(myfield) + "/>" : "<input type='" + myfield.type + "' " + A3(myfield) + "/><span class='input-group-addon'>@</span>")
                            + '</div><span style="display:none;" class="k-widget k-tooltip k-tooltip-validation k-invalid-msg" data-for="' + myfield.name + '" role="alert"></span>';
                            break;
                        case "url":
                            var additionalMarkup = '<div class="input-group">'
                            + (!rtl ? "<span class='input-group-addon'>Url</span><input type='" + myfield.type + "' " + A3(myfield) + "/>" : "<input type='text' " + A3(myfield) + "/><span class='input-group-addon'>Url</span>")
                            + '</div><span style="display:none;" class="k-widget k-tooltip k-tooltip-validation k-invalid-msg" data-for="' + myfield.name + '" role="alert"></span>';
                            break; //"percent"
                        case "percent":
                            var additionalMarkup = '<div class="input-group" style="min-width: 10em;width: 12em;">'
                            + (rtl ? "<span class='input-group-addon'>%</span><input type='number' " + A3(myfield) + " style='width:100%;'/>" : "<input type='number' " + A3(myfield) + " style='width:100%;'/><span class='input-group-addon'>%</span>")
                            + '</div><span style="display:none;" class="k-widget k-tooltip k-tooltip-validation k-invalid-msg" data-for="' + myfield.name + '" role="alert"></span>';
                            break;
                        case "autocomplete":
                            var additionalMarkup = '<input  type="autocomplete"  isInAutoComplete="true"' + A3(myfield)
                            + (myfield.codeName ? ' codeName="' + myfield.codeName : "") + '/>';
                            break;
                        case "label":
                            var additionalMarkup = '<label ' + A3(myfield) + '>'
                            + (model && model[myfield.name] != undefined ? model[myfield.name] : "") + '</label>';
                            break;
                        default:
                            var additionalMarkup = "<input type='" + myfield.type + "' " + A3(myfield) + "/>";
                    }
                }
                return additionalMarkup + datalistmarkup;
            }

            function getLookUpCodes(codeName) {
                var list = [];
                if (lookupCodes.length > 0) {
                    for (var i = 0; i < lookupCodes.length; i++) {
                        if (lookupCodes[i].CodeName == codeName) {
                            list.push(lookupCodes[i]);
                        }
                    }
                }
                return list;
            }
            // #endregion
            // #endregion

            // #region rendering

            //the intial markup is empty
            var markup = "";
            // rendering a form  is the default behavior 
            // #region rendering optional form

            if (HasFormTag) {
                markup += "<form action = '" + FormAction + "' method = '" + FormMethod + "' " + (mode == "show" ? "mode = 'show' " : "")
                + "version='" + version + "' objectName= '" + objectName + "' companyId='" + options.companyId + "' tableName='" + tableName + "' " + "FormTitle='" + formTitle + "' Culture='" + culture + "' "
                + (options.parentColumn ? " isLocal=" + (options.isLocal ? "true" : "false") + " parentColumn= '" + options.parentColumn + "' parentId='" + options.parentId + "' " : "")
                + (FormId !== undefined ? " id='" + FormId + "'>" : ">");
            }
            else {
                // notify developer that OmegaForm is used tro add fields only
                console.warn("Warrning : code 1 : \nOmegaForm Just adding new fields to an existing form .\nThe form validation and data sending operations will depend on your DOM structur \nClient validation will be disabled if you don't have a form tag ");
            }

            // #endregion
            // #region rendering optional Top panel

            // rendering a panel with title and Glyphicon is the default behavior
            //---if HasPanel
            markup += (HasPanel ? "<div class=' panel " + PanelType + "'" + (PanelId ? " id ='" + PanelId + "'" : " ") + " >"
            + "<div class='panel-heading' >" + "<div class='panel-title'>"
            + "<span  class='panelicon " + Icon + "'> </span>"
            + " <span class='paneltitle' name='" + Title + "' id='titleLbl'><span class='lblSpan'> " + (options.TitleTrls ? options.TitleTrls : Title) + " </span></span>" + "</div>" + "<hr class='visible-xs-block clearfix'/>" : "")

            //---if HasEditControls
            + (HasEditControls ? "<div class='editarea " + (HasPanel ? " col-sm-6 col-xs-12'>" : " col-xs-12 '>")
            + '<div class="switch"><input id="switchDesign" type="checkbox" ><div class="slider"></div><span></span></div>'
            + "<button type='button' id='saveDesignBtn' class='btn btn-info'><span class='glyphicon glyphicon-floppy-disk' > </span> " + (allMsgs.Save ? allMsgs.Save : "Save") + " </button>"
            + "<button type='button' id='resetBtn' class='btn btn-danger'><span class='glyphicon glyphicon-repeat' > </span> " + (allMsgs.Reset ? allMsgs.Reset : "Reset") + " </button>"
            + "</div>" + "<div class='clearfix'></div>" : "")

            //---if HasPanel
            + (HasPanel ? "</div>" + "<div class='panel-body sets-container '>" : "<div class='sets-container' >");

            // #endregion

            var SetsArray = options.FieldSets;

            //ordering sets array according to "order" property
            SetsArray.sort(reorder);

            // Iterate  through the field sets
            markup += drawSets(SetsArray);

            function drawSets(SetsArray, setModel) { //model: optional
                var markup = "";
                if (setModel) model = setModel;
                for (var counter = 0; counter < SetsArray.length ; counter++) {

                    // making currentset strongly typed :) :) ☺ ☻
                    var CurrentSet = new fieldset(SetsArray[counter]);

                    // inherites undefined properties from the form
                    if (CurrentSet.layout === undefined) CurrentSet.layout = layout;
                    if (CurrentSet.Reorderable === undefined) CurrentSet.Reorderable = reorderable;
                    if (CurrentSet.LabelEditable === undefined) CurrentSet.LabelEditable = labelEditable;

                    // adding a fieldset tag for ""logical""  grouping of fields is the default behavior 
                    // unless the ui developer set FieldSetTag it to false it'll be rendered as a div

                    var isCollapsed = localStorage.getItem("collapsedSets-" + objectName);
                    if (isCollapsed) isCollapsed = JSON.parse(isCollapsed);
                    var hasExpand = (CurrentSet.Collapsed && isCollapsed == undefined) || (isCollapsed && ($.inArray(CurrentSet.Id + '', isCollapsed) != -1));

                    markup += (CurrentSet.HasFieldSetTag ? "<fieldset " : "<div ") + (CurrentSet.legend ? "name='" + CurrentSet.legend + "'" : "")
                    + "id='" + CurrentSet.Id + "'" + " class= 'set panel " + (CurrentSet.Freez ? "frozen " : "") + (CurrentSet.LabelEditable ? "lblEditable" : "") + "'>"

                    //Set Legend
                    + (CurrentSet.legend ? (CurrentSet.HasFieldSetTag ? "<legend " : "<div ") + "class='set-title '>" : "")

                    //Collapsable Set
                    + (CurrentSet.Collapsable ? "<button type='button' class='btn btn-primary o-collabsebtn' data-parent='#" + FormId + "' data-toggle='collapse' href='#Sec" + CurrentSet.Id + "'>"
                    + "<span class='"

                    //if not in local storage -->default from db
                    + (hasExpand ? ExpandClass : CollabseClass) + "'></span>" : "")

                    //set title
                    + (CurrentSet.legend != undefined ? "<span class='lblSpan'>" + CurrentSet.legendTitle + "</span>" : "")
                    //close collapse button 
                    + (CurrentSet.Collapsable ? "</button>" : "")
                    + (CurrentSet.legend != undefined ? (CurrentSet.HasFieldSetTag ? "</legend>" : "</div>") : "") //end set title

                    // a div that has the fieldset sections that can be expanded and collabsed
                    + "<div id='Sec" + CurrentSet.Id + "' class='sections panel-body "

                    //if not in local storage -->default from db
                    + (hasExpand ? " collapse ' >" : " collapse in'>");


                    //Sections
                    markup += drawSections(CurrentSet.Sections);

                    // out of sections loop
                    // the end tag of fieldset div
                    markup += "</div>"
                    + (CurrentSet.HasFieldSetTag ? "</fieldset>" : "</div>");  // the end tag of field set

                }

                return markup;
            }
            //out of fieldset loop


            function drawSections(sections, secModel) {
                if (secModel) model = secModel;
                var markup = '';
                sections.sort(reorder);
                //iterate through the sections of the current field set
                // Sections loop start
                for (var s = 0 ; s < sections.length ; s++) {

                    if (sections[s].layout === undefined) sections[s].layout = layouts.horizontal;

                    var CurrentSection = new section(sections[s]);
                    CurrentSection.Freez == undefined ? false : CurrentSection.Freez;

                    markup += "<div " + (CurrentSection.name != undefined ? "name='" + CurrentSection.name + "'" : "")
                    + " id='" + CurrentSection.Id + "' class='" + CurrentSection.layout + " section row "
                    + (CurrentSection.Freez ? "frozen " : "") + "'>"
                    + (CurrentSection.SectionTitle != undefined && CurrentSection.layout == layouts.inline ? "<label  class='section-title col-md-" + CurrentSection.labelmd + " col-lg-" + CurrentSection.labellg + "'>" + CurrentSection.SectionTitle + "</label>" : "");

                    var fields = CurrentSection.fields;
                    fields.sort(reorder);
                    // Iterate  through the fields of the current section
                    // fields loop starts
                    for (var i in fields) {
                        // making current field strongly typed :) :) ... checks properties ... sets defaults
                        var CurrentField = new field(fields[i]);
                        if (CurrentField.name === undefined) {
                            console.error("the name property must be set for each field for the OmegaForm library to work well !!!");
                        }

                        if (CurrentField.DefaultValue != undefined) {
                            if (/^@/.test(CurrentField.DefaultValue) == true)
                                CurrentField.DefaultValue = (SessionVars[CurrentField.DefaultValue] != undefined ? SessionVars[CurrentField.DefaultValue] : CurrentField.DefaultValue);
                            else if (CurrentField.type == "multiselect")
                                CurrentField.DefaultValue = CurrentField.DefaultValue.split(',');
                            else if (CurrentField.type == "checkbox")
                                CurrentField.DefaultValue = (myfield.DefaultValue == "true");
                        }

                        if (!CurrentField.isVisible && CurrentField.DefaultValue != undefined) {
                            CurrentField.type = "hidden";
                            CurrentField.value = CurrentField.DefaultValue;
                        }

                        //Role
                        if (options.HiddenColumns && options.HiddenColumns.indexOf(CurrentField.name) > -1) {
                            if (CurrentField.DefaultValue != undefined) {
                                CurrentField.type = "hidden";
                                CurrentField.value = CurrentField.DefaultValue;
                            } else continue;
                        }

                        //Hidden
                        if (CurrentField.type == "hidden")
                            markup += getinput(CurrentField);
                        else {

                            // no formgroup start 
                            if ((CurrentSection.layout == layouts.basic || CurrentSection.layout == layouts.inline) && (CurrentField.type == "checkbox" || CurrentField.type == "radio")) {

                                markup += "<div  class='form-group " + CurrentField.type
                                + (CurrentField.md != 12 ? " col-md-" + CurrentField.sm : "")
                                + "' ><label for='" + CurrentField.name + "' >" + getinput(CurrentField)
                                + "<span class='lblSpan'>" + CurrentField.label + "</span></label></div>";
                            }
                            else {
                                //start with a form-group

                                //for inline layout
                                markup += " <div class='form-group " + CurrentField.size;
                                if (CurrentSection.layout == layouts.inline) {
                                    if (CurrentSection.fieldsNumber != undefined) {
                                        var smSize = 12 / parseInt(CurrentSection.fieldsNumber);
                                        markup += " col-md-" + smSize + " col-lg-" + smSize;
                                    } else {
                                        if (CurrentField.type != 'button') markup += " col-md-" + CurrentField.md + " col-lg-" + CurrentField.lg;
                                    }
                                }

                                ///Labels
                                // horizintal and checks & radios
                                if (CurrentSection.layout == layouts.horizontal && (CurrentField.type == "checkbox" || CurrentField.type == "radio")) {
                                    markup += "'>"
                                    + "<div class='form-group " // col-sm-" + (12 - Number(CurrentSection.labelsm)) + " col-sm-offset-" + CurrentSection.labelsm
                                    + " col-md-" + (12 - Number(CurrentSection.labelmd)) + " col-md-offset-" + CurrentSection.labelmd + " col-lg-" + (12 - Number(CurrentSection.labellg)) + " col-lg-offset-" + CurrentSection.labellg + "'>"
                                    + "<div class='" + CurrentField.type + "'>"
                                    + "<label >" + getinput(CurrentField) + "<span class='lblSpan'>" + CurrentField.label + "</span></label></div>"  ////---for editable
                                    + "</div>";
                                }
                                    // horizintal with no checks && radios
                                else if ((CurrentSection.layout == layouts.horizontal || CurrentSection.layout == layouts.inline) && CurrentField.type != "checkbox" && CurrentField.type != "radio") {
                                    markup += "'>"

                                    + " <label  for='" + CurrentField.name + "' "
                                    + "class='control-label col-md-" + CurrentSection.labelmd + " col-lg-" + CurrentSection.labellg
                                    + (CurrentField.SrOnly ? " sr-only" : "")

                                    + "'>" + "<span class='lblSpan'>" + CurrentField.label + "</span></label></" + "</label>" ////end label
                                    //field
                                    + "<div"
                                    + (CurrentField.type != 'button' ? " class=' col-md-" + (12 - Number(CurrentSection.labelmd)) + " col-lg-" + (12 - Number(CurrentSection.labellg)) + "'" : "")
                                    + ">" + getinput(CurrentField) + "</div>";
                                }
                                else {
                                    if (CurrentField.sm == 12 || CurrentSection.layout == layouts.basic) CurrentField.sm = undefined;

                                    markup += (CurrentField.sm != undefined ? " col-md-" + CurrentField.md + " col-lg-" + CurrentField.lg : "") + " '>"
                                    //label
                                    + " <label for='" + CurrentField.name + "' " + "class= '" + (CurrentField.SrOnly ? " sr-only" : "")
                                    + (CurrentField.sm != undefined ? " col-md-" + CurrentSection.labelmd + " col-lg-" + CurrentSection.labellg : "")
                                    + " '><span>" + CurrentField.label + "</span></label> "

                                    //field
                                    + (CurrentField.sm != undefined ? "<div class='" + " col-md-" + (12 - Number(CurrentSection.labelmd)) + " col-lg-" + (12 - Number(CurrentSection.labellg)) + "'>" : "");

                                    if (CurrentField.sm != undefined) CurrentField.class += " fill ";

                                    markup += getinput(CurrentField) + (CurrentField.sm != undefined ? "</div>" : "");

                                }
                                markup += "</div> ";
                            }
                        }
                    }
                    // out of fields loop

                    // the end tag of section
                    markup += "</div>";
                }

                return markup;
            }


            // the end of the genrated markup

            markup += "<input type='hidden' id='Id' name='Id' value='" + (modelId ? modelId : 0) + "' /> "
            + (model && model.CompanyId != undefined ? "<input type='hidden' id='CompanyId' name='CompanyId' value='" + model.CompanyId + "' />" : "")
            + "<div id='notify-" + objectName + "' ></div>"
            + "</div>" + (HasPanel == true ? "</div>" : "") // the close tag of set-container body
            //+ "</div>" + (HasPanel == true ? "<div class='panel-footer'>" + (btns.length ? Array(btns).indexOf("btns")) + "</div>" : "");
            + (HasFormTag ? "</form>" : "") // close form tag
             //for add option in drop down list
            + (allowInsetCode ? '<div id="addSelectWindow"></div>' : "")

            //Pop Up for column prop & roles
            + (HasEditControls && HasFormTag && admin == 'True' ? '<div class="modal fade" id="InfoPopupForm-' + objectName + '" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="true" aria-hidden="true"><div class="modal-dialog modal-xl" role="document"><div class="modal-content"><div class="modal-header"><button type="button" class="close" id="InfoBtn" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button><h4 class="modal-title" id="FormModalLabel">ColumnProperties</h4></div><div id="FormColumnProp-' + objectName + '"  class="modal-body" tabindex="0"></div></div></div></div>' : "");

            // clean up the container inner html then append new markup
            kendo.culture(culture);
            $(this).addClass("omegaform");
            $(this).html(markup);
            if (rtl) $(this).addClass("rtl");

            // #endregion

            // Context contains a jquery object of the top parent element of the generated markup
            // #region Get the markup context 

            var context = $(this);
            // #endregion

            // Html Dom H1 :) some fixes is executed after injecting the generated markup
            // #region Dom fixes

            //-------------Select - Add New-------------
            if (allowInsetCode) {
                ///for Dublication Problem
                var addClicks = [], events = $('body').data('events').click;
                if (events && events.length)
                    addClicks = events.filter(function (item) { return item.selector == ':button.k-button.addOptionF'; });

                if (addClicks.length == 0) {
                    $('body').on('click', ':button.k-button.addOptionF', function (e) {
                        e.preventDefault();
                        var newVal = $(e.target).parents(".k-nodata").prevAll(".k-list-filter").children(".k-textbox").val();
                        if (newVal != "") {
                            var formId = $(e.target).attr("formid");
                            var element = $("#" + formId + " #" + $(e.target).attr("elementname")).first();
                            var Label = $(element).parents(".form-group").find(".lblSpan").text();

                            var formData = {};
                            formData._Name = newVal;

                            var codeName = $(element).attr('codename'), isUserCode = $(element).attr('usercode') != undefined, validator;
                            if (codeName && !isUserCode) {
                                formData.IsLookUp = 1; //code
                                formData.SourceName = codeName;
                                saveOption();
                            }
                            else {
                                $("#addSelectWindow").kendoWindow({
                                    title: allMsgs.AddNew,
                                    actions: ["Minimize", "Maximize", "Close"],
                                    minWidth: "35%",
                                    visible: false,
                                    close: function () {
                                        $("#addSelectWindow").empty();
                                    }
                                });

                                var objectName = $(element).attr('objectname');
                                if (objectName) {
                                    formData.IsLookUp = 3; //object
                                    formData.SourceName = objectName;
                                    $.get("/Pages/GetColumns", { objectname: objectName }, function (result) {
                                        var qualGroupLst = objectName == "Qualifications" ? result.qualGroupLst : undefined;
                                        DrawFormWindow(result.columns, qualGroupLst);
                                    });
                                }
                                else if (isUserCode) {
                                    formData.IsLookUp = 2; //user code
                                    formData.SourceName = codeName;
                                    $.get("/LookUpCode/ReadSysCodeId", { CodeName: codeName }, function (list) {
                                        var data = []; //name, sysCode
                                        var nameField = new field({ name: "Name", type: "text", label: allMsgs.Name, value: newVal, formreq: "formreq" });
                                        var sysCodeField = new field({ name: "SysCodeId", type: "select", label: allMsgs.SysCodeId, formreq: "formreq" });
                                        data = [nameField, sysCodeField];
                                        DrawFormWindow(data);

                                        for (var i = 0; i < list.length; i++) {
                                            list[i].id = list[i].value;
                                            list[i].name = list[i].text;
                                        }
                                        fillOptionsDynamic($("#addSelectWindow").find("form"), "SysCodeId", list, null, { hasFilter: false });
                                    });
                                }
                            }

                            function DrawFormWindow(data, qualGroupLst) {
                                if (data && data.indexOf("false,") == 0) {
                                    toastr.error(data.split(",")[1]);
                                    return;
                                }
                                var windowBody = $("#addSelectWindow");
                                windowBody.attr("tablename", data[0].TableName);
                                var modalMarkup = "<form class='form-horizontal omegaform " + (rtl ? "rtl" : "") + "' tablename='" + data[0].TableName + "'><fieldset class='set panel'>";
                                for (var i = 0; i < data.length; i++) {
                                    var currentField = new field(data[i]);
                                    if (currentField.name == undefined) currentField = castGridColumns(currentField); //if Grid
                                    currentField.Id = currentField.name;
                                    modalMarkup += "<div class='form-group'><label  class='col-md-2' for='" + currentField.name + "'>" + currentField.label
                                    + "</label><div class='col-md-8'>" + getinput(currentField) + "</div></div>";
                                }
                                modalMarkup += "</fieldset><button class='btn btn-primary' id='saveAddOption'>" + allMsgs.Save + "</button></form>";
                                windowBody.html(modalMarkup);
                                windowBody.find(".form-control").not("select").css("height", "15px");
                                windowBody.css("min-height", "100%");

                                if (qualGroupLst) FormJs.fillOptionsDynamic($(windowBody).find("form"), "QualGroupId", qualGroupLst, null, { hasFilter: false });

                                DropDownList(windowBody, true); //for drop down lists - from lookupcode
                                UseKendo(windowBody); //for kendo dates & time
                                if (windowBody.find("#StartDate").length) windowBody.find("#StartDate").data("kendoDatePicker").value("01/01/2010");
                                windowBody.find("[name='Name']").val(newVal); //bind name

                                windowBody.data("kendoWindow").center().open(); //show window
                                validator = $(windowBody).kendoValidator().data("kendoValidator");

                                $(windowBody).find('#saveAddOption').click(saveOption);
                            }

                            function saveOption(event) {
                                if (event) event.preventDefault();
                                if (objectName || formData.IsLookUp == 2) {
                                    var windowBody = $("#addSelectWindow");
                                    formData._Name = windowBody.find("[name='Name']").val();
                                    formData.ColumnsNames = [];
                                    formData.ColumnsValue = [];
                                    windowBody.find("input[name], select[name]").not(":button").each(function (index, node) {
                                        //Model Values
                                        formData.ColumnsNames.push(node.name);
                                        if (node.type != "checkbox") formData.ColumnsValue.push(node.value);
                                        else formData.ColumnsValue.push(node.checked);
                                    });
                                }

                                if (validator == undefined || validator && validator.validate()) {
                                    $.post('/Pages/AddOption', { model: formData }, function (data) {
                                        postSuccessFunc(windowBody, e.target, data, function (savedData) {
                                            if (windowBody) windowBody.data("kendoWindow").close();

                                            var dropDown = $(element).data("kendoDropDownList");
                                            dropDown.dataSource.add({ id: savedData._Id, name: savedData._Name });
                                            dropDown.value(savedData._Id);
                                            $(element).trigger('change');
                                        });
                                    });
                                }
                            } //end saveOption
                        }
                    });
                }
            }

            function castGridColumns(column) {
                column.name = (column.name == undefined ? column.ColumnName : column.name);
                column.order = column.ColumnOrder;
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
                if (column.OrgInputType == "select")
                    column.type = "select";

                return column;
            }
            //---------------------------------------

            function parseServerDateWithCulture(date) {
                if (date && date.indexOf('/Date') != -1) return new Date(parseInt(date.substr(6))).toLocaleDateString(culture);
                else return date;
            }

            $(context).find("textarea").each(function () {
                $(this).css("max-width", $(this).css("width"));
            });

            $(context).find("select,input[type='range'],input[list]").each(function () {
                $(this).css("cursor", "pointer");
            });

            $(context).find("[disabled],[readonly]").each(function () {
                $(this).css("cursor", "no-drop");
            });
            // #endregion 


            //-------------Flex Columns-------------
            if (options.HasCustCols) {
                //##ToDo:
                $.get('/Flex/GetFormFlexData', { objectName: objectName, sourceId: (model.Id ? model.Id : 0), version: version }, function (result) {
                    if (result && result.FlexData) {
                        var FlexData = result.FlexData, sections = [];

                        if (FlexData.length) {
                            lookupCodes = lookupCodes.concat(result.Codes);

                            for (var i = 0; i < FlexData.length; i++) {
                                var flexField = FlexData[i];
                                flexField.type = GetFlexType(flexField.InputType);
                                //flexField.flexId = flexField.Id;
                                //flexField.Id = flexField.name;
                                model[flexField.name] = (flexField.type != 'select' ? flexField.Value : flexField.ValueId);
                                var obj = {
                                    Id: i, name: flexField.name + '_flexSec', fields: [flexField], order: flexField.order,
                                    Freez: true, layout: (flexField.InputType == 7 ? layouts.horizontalinline : layouts.inline)
                                };
                                sections.push(obj);
                            }

                            var flexObj = [{
                                Id: 0, layout: layouts.horizontal, LabelEditable: true, Collapsable: true, HasFieldSetTag: true, Freez: true,
                                legend: "AdditinalColumns", legendTitle: result.Legend, Sections: sections
                            }];

                            ///Render AdditinalColumns FieldSet and Columns.
                            $(context).find('.set:last').before(drawSets(flexObj));

                            var flexContext = $(context).find('.set[name="AdditinalColumns"]').addClass('flex-columns');

                            flexContext.find(":input[formreq]").closest(".form-group").find(".lblSpan").css("color", "#d50505");
                            UseKendo(flexContext);
                            DropDownList(flexContext, false);
                        }
                    }
                });


                function GetFlexType(Type) {
                    var type = '';
                    switch (Type) {
                        case 2: type = 'number'; break;
                        case 3: type = 'select'; break;
                        case 4: type = 'date'; break;
                        case 5: type = 'time'; break;
                        case 6: type = 'datetime'; break;
                        case 7: type = 'textarea'; break;
                        case 8: type = 'checkbox'; break;
                        default: type = 'text'; break;
                    }
                    return type;
                }
            }
            //-------------End Flex Columns-------------


            if (mode == "show") {
                var submitBtn = $(context).find('.submit');
                if (submitBtn.parents(".section").children().not(".ModifyInfo").length == 1)
                    submitBtn.parents('.set').remove();
                else
                    submitBtn.parents(".form-group").remove();
            }
            //#region validation
            var validator = $(context).find("form").kendoValidator().data("kendoValidator"),
                status = $(context).find(".status");

            ///##region Kendo UI
            function DropDownList(context, isDynamic) {
                //--Select - DropDownList
                var selectField = $(context).find("select[codename]");
                if (selectField.length != 0) {
                    for (var i = 0; i < selectField.length; i++) {
                        var elementName = $(selectField[i]).attr("name");
                        var codeName = $(selectField[i]).attr("codename");
                        if (codeName) {
                            if ($(context).find("form").length > 0) context = $(context).find("form");

                            if (isDynamic == true) { //for Options Popup
                                $.ajax({
                                    async: false,
                                    url: "/Pages/GetCodeList?codeName=" + codeName,
                                    success: function (list) {
                                        fillOptionsDynamic(context, elementName, list, null, { hasFilter: false });
                                    }
                                });
                            }
                            else {
                                var list = getLookUpCodes(codeName);
                                fillOptionsDynamic(context, elementName, list, model);
                            }
                        }
                    }
                }
            }



            function UseKendo(context) {
                var calender,
                    dateField = $(context).find("input[type=" + FieldTypes.Date + "]").filter("[disable-weekend],[disable-holidays]");
                if (dateField.length) {
                    $.get("/Pages/GetCalender", null, function (res) { calender = res; });
                }

                function disableWeekEnd(date) {
                    if (calender && date && (date.getDay() == calender.weekend1 || date.getDay() == calender.weekend2)) return true;
                }

                function disableHolidays(date) {
                    var isDisabled;

                    if (date && calender) {
                        //Custom Holidays

                        for (var i = 0; i < calender.CustomHolidays.length; i++) {
                            var holiDate = new Date(calender.CustomHolidays[i].HoliDate);
                            if (holiDate.getYear() == date.getYear() && holiDate.getMonth() == date.getMonth() && holiDate.getDate() == date.getDate()) {
                                isDisabled = true;
                                break;
                            }
                        }

                        //Standard Holidays
                        var sholiday = calender.StanderdHolidays;
                        for (var i = 0; i < sholiday.length; i++) {
                            if ((date.getMonth() + 1) == sholiday[i].SMonth && date.getDate() == sholiday[i].SDay) {
                                isDisabled = true;
                                break;
                            }
                        }
                    }
                    return isDisabled;
                }
                $(context).find("input[type=" + FieldTypes.Date + "]").attr("data-type", 'date').kendoDatePicker({ culture: culture, value: parseServerDate($(this).val()) });

                for (var i = 0; i < dateField.length; i++) {
                    var field2 = $(dateField[i]);
                    if (field2.filter('[disable-weekend][disable-holidays]').length > 0) {
                        field2.data('kendoDatePicker').setOptions({
                            disableDates: function (date) {
                                if (disableWeekEnd(date)) return true;
                                if (disableHolidays(date)) return true;
                            }
                        });
                    }
                    else if (field2.filter('[disable-weekend]').length > 0) {
                        field2.data('kendoDatePicker').setOptions({ disableDates: disableWeekEnd });
                    }
                    else if (field2.filter('[disable-holidays]').length > 0) {
                        field2.data('kendoDatePicker').setOptions({ disableDates: disableHolidays });
                    }
                }

                //--Date & Time Pickers 
                $(context).find("input[type=" + FieldTypes.DateTime + "]").attr("data-type", 'datetime').kendoDateTimePicker({ culture: culture });
                $(context).find("input[type=" + FieldTypes.Time + "]").attr("data-type", 'time').kendoTimePicker({ culture: culture, format: 'h:mm tt', parseFormats: ["HH:mm"], dateInput: true });

                //--MaskedTextBox
                var maskedField = $(context).find("input[type=" + FieldTypes.MaskedTextBox + "]");
                maskedField.kendoMaskedTextBox({
                    mask: maskedField.attr("mask")
                });

                //--MultiSelect
                //$(context).find("select[multiple][codename]").kendoMultiSelect().data("kendoMultiSelect");
            }

            DropDownList(context, options.isDynamic);
            UseKendo(context);

            ///##endregion Kendo UI
            //--AutoComplete
            // isInAutoComplete="true"
            var autoComplete = $(context).find("input[type='autocomplete']");
            if (autoComplete.length != 0) {
                for (var i = 0; i < autoComplete.length; i++) {
                    var codeName = $(autoComplete[i]).attr("codename");
                    if (codeName) {
                        var list = getLookUpCodes(codeName);
                        fillOptionsDynamic($(context).find("form"), $(autoComplete[i]).attr("name"), list, model);
                    }
                }
            }

            //Remove Label from Button fields
            var btns = $(context).find("input[type=button]").add("input[type=submit]");
            btns.addClass("button");
            btns.parents(".form-group").children("label").remove();

            //-------creation User & Date-------
            var htmlMarkup = '<div class="ModifyInfo">'
            //Created
            + (model && model.CreatedUser ? '<div><b>' + allMsgs.CreatedUser + ':&nbsp;</b> ' + model.CreatedUser + '&nbsp;&nbsp;-&nbsp;&nbsp;'
                + (model.CreatedTime ? '<b>' + allMsgs.CreatedTime + ':&nbsp;</b> ' + parseServerDateWithCulture(model.CreatedTime) + '</div>' : "") : "")
            //Modified
            + (model && model.ModifiedUser ? '<div><b>' + allMsgs.ModifiedUser + ':&nbsp;</b> ' + model.ModifiedUser + '&nbsp;&nbsp;-&nbsp;&nbsp;'
                + (model.ModifiedTime ? '<b>' + allMsgs.ModifiedTime + ':&nbsp;</b> ' + parseServerDateWithCulture(model.ModifiedTime) + '</div>' : "") : "")
            + '</div>';

            $(context).find(".set .section").last().append(htmlMarkup);
            //--------------------------------

            //Documents & BackToIndex Buttons
            //if (formBtns.length) {
            //    var container = ($(context).closest(".tab-content").length ? $(context).closest(".tab-content") : $(context).find(".sets-container"));

            //    var BtnsTxt = (formBtns.indexOf("doc") != -1 ? "<button id='Documents' name='Documents' onClick='return false;' class='btn btn-info ajaxBtn button' accesskey='d'><span class='fa fa-link'></span> " + (allMsgs.Documents ? allMsgs.Documents : "Documents") + " <span id='nofdocs' class='badge badge-red'>" + (model && model.Attachments ? model.Attachments : 0) + "</span></button>" : "")
            //    + (formBtns.indexOf("back") != -1 ? "<button id='backToIndex' name='backToIndex' onClick='return false;' class='btn btn-warning back button ' accesskey='b'> " + (allMsgs.backToIndex ? allMsgs.backToIndex : "Back To Index") + " </button>" : "")

            //    if (container.find("#Documents, #backToIndex").length == 0) {
            //        container.append("<div id='btnsDiv' class='form-inline section row frozen'>" + BtnsTxt + "</div>");
            //        $(container).on('click', '#backToIndex', function () {
            //            //var oldPage = localStorage.getItem("menuhigh").split(",");
            //            //var oldulr = $("#" + oldPage[2] + " a").attr("href");
            //            // $("#renderbody").load(oldulr);
            //            updateHistory(oldUlr);
            //        });
            //    }
            //}

            //----------------Formula-----------------
            bindFormulaFunc();

            function bindFormulaFunc() {
                var columns = $(context).find('[formula]');

                formulaSelectors.forEach(function (selector) {
                    $(context).on('change', selector, baseChanged);
                    baseChanged(); /// for load - intial
                    function baseChanged() {
                        var fields, formulafield, value, baseColumns = $(selector); //baseColumn-> Salary, fields-> Salary50%
                        baseColumns.each(function (index, item) {
                            model[item.id] = (mode == "show" ? $(item).text() : $(item).val());
                        });

                        fields = columns.filter('[formula-columns="' + selector + '"]');

                        for (var i = 0; i < fields.length; i++) {
                            var formula, field2 = $(fields[i]), fieldName = field2.attr("id");

                            var foundObj = formulaObjArr.filter(function (item) { return item.column == fieldName; });
                            if (foundObj.length) formula = foundObj[0].formula;

                            try {
                                value = eval(formula);
                            }
                            catch (ex) {
                                console.log(ex);
                            }

                            //1-validate value(undefiend | NaN | ...). 2-field2 type(input | label | ...).
                            if (value != NaN) {
                                if (!isNaN(Number(value))) { //is Number
                                    value = parseFloat(value).toFixed(2);
                                    //if (value % 1 == 0)  value = parseInt(value); ///is Int
                                }
                                //Bind Value
                                if (field2.length && field2[0].localName == "label") field2.text(value);
                                else field2.val(value);
                            }
                        }
                    }
                });
            }
            //----------------End Formula-----------------

            // intialize the edit mode functionality with out mixing the logic with the UI switcher element
            // to be called on startup or when the developer switches to edit mode :)
            var fieldsetsParent, sectionsParents;
            function intialize_Sort() {
                //initialize editing mode ... but make it disabled until the user switches to the -design- edit mode

                //-order section
                sectionsParents = $(context).find(".set").not(".frozen").children(".sections");
                $(sectionsParents).kendoSortable({
                    filter: ".sortable",
                    container: sectionsParents,
                    ignore: "input",
                    hint: hintFunc,
                    placeholder: placeholderFunc,
                    change: function () {
                        if (!hasdesignchanges) designchanged(true);
                    }
                });

                //-order fieldset
                fieldsetsParent = $(context).find(".sets-container")[0];
                $(fieldsetsParent).kendoSortable({
                    filter: ".sortable.set",
                    container: fieldsetsParent,
                    ignore: "input",
                    hint: hintFunc,
                    placeholder: placeholderFunc,
                    change: function () {
                        if (!hasdesignchanges) designchanged(true);
                    }
                });

                function hintFunc(e) {
                    return e.clone().addClass("hint")
                       .height(e.height()).width(e.width());
                }

                function placeholderFunc(e) {
                    return e.clone().addClass("placeholder");
                }

                var sections = $(context).find(".set").not(".frozen").children(".sections");

                IntializedForSort = true;
            }

            removeSortable();

            function Switch_Reorder(on) {
                if (on) {
                    if (!IntializedForSort) intialize_Sort();
                    addSortable();
                    $(context).find(".sortable").hover(InOut);
                }
                else {
                    if (IntializedForSort) {
                        $(context).find(".sortable").off("mouseenter mouseleave", InOut);
                        //Remove Sortable class to stop sort
                        removeSortable();
                    }
                }
                reorderable = on;
            }

            function addSortable() {
                $(context).find(".set").not(".frozen").children(".sections").children(".section").not(".frozen").addClass("sortable");
                $(context).find(".sets-container").children().not(".frozen").addClass("sortable");
            }
            function removeSortable() {
                $(context).find(".set").not(".frozen").children(".sections").children().not(".frozen").removeClass("sortable");
                $(context).find(".sets-container").children().not(".frozen").removeClass("sortable");
            }

            ///Editable (pencil menu - [edit label, remove field, notification])
            function disable_editing(container, id, _editables) {
                $(container).find("button.edit-bt").removeClass("btn btn-xs glyphicon glyphicon-pencil");
                $(container).find("button.edit-bt").remove();
                $(container).find("[id='PopupMenu']").remove();
            }

            $("#notify-" + objectName).kendoWindow({
                minWidth: "500px",
                width: "55%",
                height: "90%",
                actions: ["Minimize", "Maximize", "Close"],
                visible: false,
                close: function () {
                    $("#notify-" + objectName).empty();
                },
                animation: {
                    open: {
                        effects: "slideIn:left fadeIn",//"expand:vertical", //"slideIn:top fadeIn", //":top fadeIn",
                        duration: 500
                    }
                }
            });

            function enable_editing(container, id, editable, autoSavetoServer) {
                /// <summary>Enable editing of labels , links , ...etc or the selector defined by the _editables paramter saves it to the local storage</summary>
                /// <param name="container" type="jquery">the html element that contains elements to edit</param>
                ///<param name="_editables" type="jquery selector">the html elements that needs to be editable the default value is "label,a"</param>
                /// <returns type=""></returns>

                var temp = window.location.pathname.split("/");
                // the controller name + the action name
                var uniquename = temp[1] + "_" + temp[2];

                if (id === undefined) id = $(container).attr("id");

                editable.each(function () {
                    $(this).css("display", "inline");
                    $(this).attr("data-default", $(this).text());
                    //auto generate IDs if it doesn't exist
                    if ($(this).attr("id") === undefined) {
                        //get the first parent that has id  - H1 idea :)
                        var parentid = $(this).parents("[id]").first().attr("id");
                        //get the index of the current editable element
                        var el_index = $(this).parents("[id]").first().find(editable).index($(this));
                        $(this).attr("id", parentid + "_" + el_index);
                    }
                });

                // injecting new dom element to trigger (editing, notification, remove )
                editable.prepend('<div id="PopupMenu" class="btn-group"><button type="button" class="edit-bt btn btn-xs glyphicon glyphicon-pencil dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"></button><ul class="dropdown-menu"><li><a class="ajaxBtn editOption"><i class="fa fa-edit"></i><span>&nbsp; ' + (allMsgs.editLbl || 'Edit Label') + '</span></a></li><li><a class="ajaxBtn removeRec"><i class="fa fa-remove"></i><span>&nbsp; ' + (allMsgs.removeRec || 'Remove Record') + '</span></a></li>'
                    + '<li><a class="ajaxBtn columnProp"><i class="fa fa-cog"></i><span>&nbsp; ' + (allMsgs.ColumnProperties || 'Column Properties') + '</span></a></li>'
                    + (model && model.Id ? '<li><a class="ajaxBtn notify"><i class="fa fa-bell"></i><span>&nbsp; ' + (allMsgs.notify || 'Add Notification') + '</span></a></li>' : '') + '</ul></div>');

                //// add Reminder Me to date items only
                //editable.next().has("input[data-role^='date']").prev().find("#PopupMenu ul.dropdown-menu").append('<li><a class="ajaxBtn reminderMe"><i class="fa fa-clock-o"></i><span>&nbsp; ' + (allMsgs.reminderMe ? allMsgs.reminderMe : 'Reminder Me') + '</span></a></li>');

                //remove notify from set and panel header
                $(context).find(".set-title .dropdown-menu").find("a.notify, a.columnProp").remove();
                $(context).find(".paneltitle, .set.flex-columns").find(".dropdown-menu").find("a.notify, a.columnProp, a.removeRec").remove();

                //------------Events------------
                ///remove form columns & field sets
                $(context).find(".set-title .edit-bt").click(function (e) {
                    e.stopPropagation();
                    $(this).next(".dropdown-menu").toggle();
                });

                editable.find("a.editOption").click(function (e) {
                    e.stopPropagation();
                    e.preventDefault();
                    if (!hasdesignchanges) designchanged(true);
                    doedit(this, editable);
                });

                editable.find("a.removeRec").click(function (e) {
                    e.stopPropagation();
                    e.preventDefault();
                    if (!hasdesignchanges) designchanged(true);
                    removeFeild(this);
                });

                editable.find("a.notify, a.columnProp").click(function (e) {
                    e.stopPropagation();
                    e.preventDefault();
                    var windTitle, url, formAttr = $(context).find("form"),
                        columnName = $(this).closest(".form-group").find(":input[name]").attr("name"),
                        modelId = $(context).find("#Id").val(); //for notify for current record only

                    if ($(this).hasClass('columnProp')) { // column prop
                        windTitle = allMsgs.ColumnProperties || 'Column Properties';
                        url = "/Pages/FormColumnPropForm?tableName=" + tableName + "&objectName=" + objectName + "&columnName=" + columnName + "&version=" + formAttr.attr("version");
                    }
                    else { //Notifications
                        windTitle = allMsgs.Notifications || "Notifications";
                        url = "/Notification/NotificationMenu?TableName=" + tableName + "&ObjectName=" + objectName + "&ColumnName=" + columnName + "&Version=" + formAttr.attr("version") + "&Type=Form&CurrentId=" + modelId
                    }
                    var Wind = $("#notify-" + objectName).data("kendoWindow");
                    Wind.title(windTitle);
                    Wind.refresh(url).center().open();
                });
            }

            function Switch_Label_Editor(on) {
                var editables = $(context).find(".lblEditable").find(".lblSpan").parent().not(".radio, :hidden").add($(context).find("#titleLbl"));

                if (on) {
                    enable_editing(context, FormId, editables, true);
                    //$(context).find("button.edit-bt").addClass("btn btn-xs glyphicon glyphicon-pencil");
                    $(context).find(".lblSpan").on("keydown", function (e) {
                        if (e.keyCode == 32) {
                            var span = $(this);
                            e.preventDefault();
                            var position = window.getSelection().getRangeAt(0).startOffset;
                            var text = [span.text().slice(0, position), '&nbsp;', $(this).text().slice(position)].join('');
                            span.html(text);

                            var range = document.createRange();
                            var sel = window.getSelection();
                            range.setStart(span[0].childNodes[0], ++position);
                            range.collapse(true);
                            sel.removeAllRanges();
                            sel.addRange(range);
                        }
                    });
                }
                else {
                    disable_editing(context, FormId, editables);
                }
            }
            //edit labels
            function doedit(target, editables) {
                var el = $(target).closest("#PopupMenu").nextAll(".lblSpan");      //.nextAll(editables).not(":input");
                el.attr('contenteditable', 'true');
                el.attr("spellcheck", "false");

                //Selection
                var child = $(this).children();
                var range = document.createRange();
                range.setStart(el[0], 1);
                range.setStart(el[0], child.length);
                var sel = window.getSelection();
                sel.removeAllRanges();
                sel.addRange(range);
                //End Selection

                el.focus();
            }

            ///Old remove before pencil menu
            //function toggleRemove() {
            //    var btnClass = " btn-danger";
            //    if ($(this).hasClass("set")) btnClass = " btn-warning";
            //    var btn = "<button class='btn-remove btn btn-xs" + btnClass + "'>x</button>";
            //    var btnSelector = $(this).find(".btn-remove");
            //    if (btnSelector.length == 0) {
            //        $(this).prepend(btn);
            //    }
            //    else
            //        btnSelector.remove();
            //}

            //sets the UI state after switching design mode on and off
            function design_mode(on) {
                if (on) {
                    $(context).find('.submit, .back').attr('disabled', true);
                    $(context).find(".editarea button").show();
                    if (HasPanel) {
                        change_form_icon(formIcons.design);
                        change_panel_type(Panels.design);
                    }
                }
                else {
                    $(context).find('.submit, .back').attr('disabled', false);
                    $(context).find(".editarea button").hide();
                    if (HasPanel) {
                        change_form_icon(originalIcon);
                        change_panel_type(originalpanel);
                    }
                }
            }

            function saveCollapsedState() {
                var set = $(context).find(".set");
                var collabsedSets = [];
                for (var i = 0; i < set.length; i++) {
                    if ($(set[i]).find(".o-collabsebtn .glyphicon").hasClass(ExpandClass))
                        collabsedSets.push(set[i].id);
                }

                localStorage.setItem("collapsedSets-" + objectName, JSON.stringify(collabsedSets));
            }

            ///Design & data Changed
            var orgFuncHandler, selectors;

            function designchanged(on) {
                hasdesignchanges = on;
                changed("Design");
            }

            function datachanged(on) {
                hasdatachanges = on;
                changed("Data");
            }

            function changed(mode) {
                var msg = mode + (allMsgs.warningSave == undefined ? " Changed" : allMsgs.warningSave);
                var modeFlag = mode == "Data" ? hasdatachanges : hasdesignchanges;
                var otherFlag = mode == "Data" ? hasdesignchanges : hasdatachanges;
                function clickHandler(e) {
                    otherFlag = mode == "Data" ? hasdesignchanges : hasdatachanges; //In first time when Ok the flag becomes false
                    var result = confirm(msg);
                    if (result) {
                        e.preventDefault();
                        e.stopPropagation();
                        selectors.off("click", clickHandler);
                        if (!otherFlag) {
                            onbeforeunload = null;
                            EventsBack();
                            $(this).click();
                        }
                        mode == "Data" ? hasdatachanges = false : hasdesignchanges = false;
                    } else {
                        e.preventDefault();
                        e.stopPropagation();
                    }

                }

                if (!modeFlag) {
                    if (mode == "Design")
                        mode = "Data";
                    onbeforeunload = null;
                    selectors.off("click");
                    if (!otherFlag) EventsBack();
                    else {
                        changed(mode);
                    }
                }
                else {
                    if (!otherFlag) {
                        onbeforeunload = function (event) {
                            event.returnValue = msg;
                            return msg;
                        };
                        var gridform = $(context).find("div.k-grid").find("a");

                        selectors = $('a, :button').not('.o-collabsebtn, #saveDesignBtn, #columnPropBtn, .edit-bt, .btn-remove, .addOption , [href="javascript:;"], .k-animation-container a, .closeAdd, .ajaxBtn').not(gridform);
                        selectors = mode == "Data" ? $(selectors).not(".submit") : $(selectors).not("#resetBtn");
                        if (orgFuncHandler == null) {
                            orgFuncHandler = [];

                            for (var i = 0; i < selectors.length; i++) {
                                if ($(selectors[i]).data("events") !== undefined) {
                                    var funArr = [];

                                    var clicks = $(selectors[i]).data("events").click;
                                    if (clicks) {
                                        for (var j = 0; j < clicks.length; j++) {
                                            funArr.push(clicks[j].handler);
                                        }
                                    }
                                    orgFuncHandler[i] = funArr;

                                } else {
                                    orgFuncHandler[i] = [];
                                }
                            }
                        }
                        selectors.off("click");
                    }
                    $(selectors).on("click", clickHandler);
                }

                function EventsBack() {
                    for (var i = 0; i < selectors.length; i++) {
                        if (orgFuncHandler[i]) {
                            for (var j = 0; j < orgFuncHandler[i].length; j++) {
                                $(selectors[i]).click(orgFuncHandler[i][j]);
                            }
                        }
                    }
                }
            }
            ///end Design & data Changed

            //Data Changed
            if (model && model.Id != 0) { $('.save-group').find('button').attr('disabled', true); $(context).find('form').attr('data-disable', false); $("#b6,#b7").attr('disabled', false); }

            $(context).find('form').on('change', ':input:not(:button, #switchDesign)', function () {
                //$(context).find(".submit").attr("disabled", false);
                $('.save-group').find('button').attr('disabled', false);
                $(context).find('form').attr('data-disable', true);
                $("#b6,#b7").attr('disabled', true);
            });

            $(context).find(':input:not(:button, #switchDesign)').change(function () {
                if (!hasdatachanges) datachanged(true);
            });

            // Default generated UI events 
            $(context).on('click', ".o-collabsebtn", function (e) {
                CollabseExpand($(this), $(this).parents(".set").children(".sections"), $(this).children("span.glyphicon"), CollabseClass, ExpandClass);
                saveCollapsedState();
            });

            // Info popup form
            $(context).find("#InfoPopupForm-" + objectName + " #InfoBtn").click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                $("#InfoPopupForm-" + objectName).modal("hide");
            });

            // slider
            if (HasEditControls) {
                var switchkey = $(context).find(".switch .slider").prev("input")[0];
                $(switchkey).change(function () {
                    if (this.checked) {
                        design_mode(true);
                        Switch_Reorder(true);
                        Switch_Label_Editor(true);
                    }
                    else {
                        design_mode(false);
                        Switch_Reorder(false);
                        Switch_Label_Editor(false);
                    }
                });
            }

            //for style sortable (border on hover)
            function InOut() {
                if ($(this).hasClass("frozen")) {
                    $(this).toggleClass("no-move");
                }
                else {
                    $(this).toggleClass("move");
                }
            }

            //calling startup methods
            if (reorderable) {
                intialize_Sort();
            }

            //change panel style
            function change_form_icon(iconclass) {
                if (HasPanel) {
                    var iconHolder = $(context).find(".panelicon");
                    var holderMarkup = "";
                    if (iconHolder[0].localName == "span" && iconclass == formIcons.design) {
                        holderMarkup = '<a id="rolesBtn" type="button" class="panelicon glyphicon glyphicon-user dropdown-toggle"></a>'
                        + '<a id="columnPropBtn" type="button" class="panelicon ' + iconclass + ' dropdown-toggle"></a>';
                    }
                    else
                        holderMarkup = "<span  class='panelicon " + iconclass + "'> </span>";

                    iconHolder.parent().prepend(holderMarkup);
                    iconHolder.remove();

                    if (HasEditControls && HasFormTag && admin == 'True') {
                        $("#columnPropBtn").click(columnPropFunc);
                        $("#rolesBtn").click(rolesFunc);
                    }
                    //$(context).find(".panelicon").first().removeClass(Icon).addClass(iconclass);
                    Icon = iconclass;
                }
                else {
                    console.warn("warnning : OmegaForm is set to have no panel so there is no icon to change !!");
                }
            }

            function change_panel_type(p) {
                if (HasPanel) {
                    $(context).find(".panel").removeClass(PanelType).addClass(p);
                    PanelType = p;
                }
                else {
                    console.warn("warnning : OmegaForm is set to have no panel so there is no panel to change !!")
                }

            }
            //end change panel style

            // reset button
            $(context).find("#resetBtn").on('click', function () {
                if (hasdesignchanges) designchanged(false);
                design_mode(false);

                var modal = $(context).closest(".modal");
                var isInModal = $(context).closest(".modal").length > 0;
                if (isInModal) {
                    modal.modal('hide');
                    $(".modal-backdrop").remove();
                }
                if (options.companyId == 0) {
                    if (!isInModal) $("#renderbody").load(ulr);
                } else {
                    var url = "/Pages/ResetForm";
                    var ObjectName = $(context).find("form").attr("objectName");
                    $.post(url, { objectname: ObjectName, version: version }, function (data) {
                        if (!isInModal) $("#renderbody").load(ulr);
                    });
                }
            });

            // save design
            $(context).find("#saveDesignBtn").on('click', function () {
                if (hasdesignchanges) designchanged(false);
                $(context).find("[id=PopupMenu] .dropdown-menu").remove();
                saveFormDesign(context);

                //Close Design Mode
                $(context).find(".switch .slider").prev("input")[0].checked = false;
                Switch_Reorder(false);
                Switch_Label_Editor(false);
                design_mode(false);
            });

            //when save, make hasdatachanges flag = false
            //$(context).find('.sets-container').on('click', '.submit', null, function (e) {
            $(".save-group").off('click').on('click', ':not(#div12345)', function (e) {
                e.preventDefault();
                if (hasdatachanges) datachanged(false);
            });
            //Alt + 'char'
            //$(context).find('.submit[name^=save], .submit[name^=Save]').filter(":eq(0)").attr("accesskey", "s");
            //$(context).find('.back[name="backToIndex"]').filter(":eq(0)").attr("accesskey", "b");
            //Log Tooltip
            $(context).on("keydown", "input, select", function (e) {
                if (model && (model.Id > 0 || typeof (model.Id) == "string" && model.Id.length > 2)) {
                    if (options.LogTooltip == true && e.keyCode === 13) {
                        var input = $(this), form = $(input).closest('form'), columnname = input.attr("name");
                        if (input.attr("aria-owns") != undefined && input.attr("aria-owns") != "") {
                            var selectInput = input.attr("aria-owns").split("_")[0];
                            columnname = selectInput;
                            form = $(context).find("#" + selectInput).closest('form');
                        }
                        $.ajax({
                            url: "/Home/Getchanges",
                            contentType: "text/html",
                            data: { objectname: form.attr("objectname"), companyId: form.attr("companyid"), version: form.attr("version"), columnname: $(this).attr("name"), sourceId: model.Id, CreatedTime: parseServerDate(model.CreatedTime), CreatedUser: model.CreatedUser },
                            success: function (res) { $(input).kendoTooltip({ content: res, width: 400, height: 200, position: "bottom" }); },
                            error: function () { }
                        });
                    }
                }
            });

            //prevent float from number where step = 1
            $(context).find('input[type="number"][step="1"]').keypress(function (e) {
                return e.charCode >= 48 && e.charCode <= 57;
            });

            //Popup columnProp Grid
            function columnPropFunc() {
                var formAttr = $(context).find("form");
                $("#InfoPopupForm-" + objectName + " #FormColumnProp-" + objectName).load("/Pages/FormColumnPropGrid?objectName=" + formAttr.attr("objectName") + "&version=" + formAttr.attr("version"));
            }
            //Popup roles Grid
            function rolesFunc() {
                var formAttr = $(context).find("form");
                $("#InfoPopupForm-" + objectName + " #FormColumnProp-" + objectName).load("/pages/RolePropGrid?objectName=" + formAttr.attr("objectName") + "&version=" + formAttr.attr("version") + "&companyId=" + formAttr.attr("companyId"));
            }
            //Tabs
            // Align Tabs
            $('#alignjustify').off('click');
            $('#alignjustify').click(function () {
                // Vertical
                if ($('#classrow').hasClass('')) {
                    $('#classrow').addClass('row');
                    $('#classcol-md-2').addClass('col-md-2 col-sm-2 col-xs-2 dummy');
                    $('#classcol-md-10').removeClass().addClass('col-md-10 col-sm-10 col-xs-10')
                    if ($('body').hasClass('rtl')) {
                        $('#menu_tabs').removeClass().addClass('nav nav-tabs tabs-right');
                    }
                    else {
                        $('#menu_tabs').removeClass().addClass('nav nav-tabs tabs-left');
                    }
                }//Horzinotal
                else if ($('#classrow').hasClass('row')) {
                    $('#classrow').removeClass('row');
                    $('#classcol-md-2').removeClass('col-md-2 col-sm-2 col-xs-2 dummy');
                    $('#menu_tabs').removeClass().addClass('nav nav-tabs');
                    $('#classcol-md-10').removeClass('col-md-10 col-sm-10 col-xs-10');
                }
            });
            // End Align

            //remove empty sections
            var allSections = $(context).find(".section");
            for (var i = 0; i < allSections.length; i++) {
                if ($(allSections[i]).children().length == 0)
                    $(allSections[i]).css("margin-bottom", "0px");
            }

            //underline required field
            $(context).find(":input[formreq]").closest(".form-group").find(".lblSpan").css("color", "#d50505");
            if ($(context).hasClass("rtl")) $(context).find(":button").closest(".section").css("direction", "rtl");

            return {
                Markup: this.markup,
                AddNewSet: drawSets,
                AddNewSection: drawSections,
                Context: context,
            };
        }
    });
    // #endregion formregion

    function saveForm(context, btn, afterSave) {
        context = $(context);
        var validator = context.data("kendoValidator");

        var isValid = context.find(".k-invalid").length == 0;
        if (isValid && validator) isValid = validator.validate();

        if (isValid) {
            ///forbidden user to click again (save changes button)
            $(".save-group").find("button").attr('disabled', true);
            $("#b6,#b7").attr('disabled', false);
            $(context).find('form').attr('data-disable', false);
            ///

            var formData = {}, visibleColumns = [], selected = {};

            selected.OldValues = oldModel[context.attr("id")];
            selected.NewValues = [];
            //Collect form data 
            context.find(":input[name]:not(:button, [flexId])").each(function (index, node) {
                //for Visible Columns
                visibleColumns.push(node.name);

                var newValue = getColumnData(node, formData);

                if (newValue.ColumnName)
                    selected.NewValues.push(newValue);
            });
            selected.visibleColumns = visibleColumns;


            //-----Save FlexData-----
            var flexFieldsSet = context.find('.set.flex-columns');
            if (flexFieldsSet.length > 0) {
                var flexData = [], SourceId = context.find('#Id').val();

                flexFieldsSet.find(":input[name]:not(:button)").each(function (index, node) {
                    var columnValue = {};
                    var newValue = getColumnData(node, columnValue);

                    ///--ValueId, Value, SourceId, ColumnName, TableName, PageId
                    var obj = {
                        flexId: $(node).attr('flexId'),
                        PageId: $(node).attr('PageId'),
                        TableName: $(node).attr('tableName'),
                        SourceId: SourceId,
                        ColumnName: node.name,
                        Value: columnValue[node.name]
                    };

                    //Dropdown list
                    if (newValue.ColumnName) {
                        obj.ValueId = columnValue[node.name];
                        obj.Value = newValue.Text;
                    }
                    flexData.push(obj);
                });
                formData["flexData"] = flexData;
            }


            //formData["moreInfo"] = selected;
            formData["version"] = context.attr("version");
            formData["visibleColumns"] = selected.visibleColumns;
            formData["NewValues"] = selected.NewValues;
            formData["OldValues"] = selected.OldValues;
            formData["clear"] = false;

            for (var i = 3; i < arguments.length; i++) {
                if (typeof arguments[i] != 'boolean')
                    formData["grid" + (i - 2)] = arguments[i];
                else
                    formData["clear"] = arguments[i];
            }

            //Send data to server
            var formAction = context.attr("action");
            if (formAction) {
                $.post(formAction, formData).done(function (data) {
                    postSuccessFunc(context, btn, data, afterSave);
                });
            } else {
                return formData;
            }
        }
        else {
            var Errors = context.find(".k-invalid");
            for (var i = 0; i < Errors.length; i++) {
                var Spanmsg = $(Errors[i]).add($(Errors[i]).closest(".input-group")).next("span.k-invalid-msg");
                if (Spanmsg.length) toastr.error(Spanmsg.text());
            }
        }
    }

    function getColumnData(node, formData) {
        var newValue = {};

        //Model Values
        if (node.autocomplete) {
            formData[node.name] = node["data-val"];
            newValue.Text = node.value;
            newValue.ColumnName = node.name;
        }
        else if (node.type == "select-multiple") {
            formData[node.name] = [];
            var selOptions = node.selectedOptions;
            newValue.ColumnName = (node.name.indexOf('I') == 0 ? node.name.substr(1) : node.name);
            newValue.Text = "";
            for (var i = 0; i < selOptions.length; i++) {
                formData[node.name].push(selOptions[i].value);
                newValue.Text += selOptions[i].text + ", ";
            }
        }
        else if (node.type == "radio") {
            if (node.checked) {
                formData[node.name] = node.value;
                newValue.ColumnName = node.name;
                newValue.Text = node.nextSibling.textContent;
            }
        }
        else if ($(node).data("kendoDatePicker") != undefined) {
            var date = $(node).data("kendoDatePicker").value();
            if (date) formData[node.name] = kendo.toString(date, "yyyy-MM-dd");
        }
        else if (node.type != "checkbox") {
            formData[node.name] = node.value;
            if (node.type == "select-one") {
                newValue.ColumnName = node.name;
                newValue.Text = (node.selectedOptions.length ? node.selectedOptions[0].text : "");
            }
        }
        else
            formData[node.name] = node.checked;
        return newValue;
    }

    ///bind errors on views 
    function postSuccessFunc(context, btn, data, afterSave) {
        if (typeof (data) == "string" && data.indexOf("OK") > -1) {
            if (afterSave) {
                var savedData = data.substring(3, data.length), savedObject;
                if (savedData != "") {
                    savedObject = JSON.parse(savedData);
                    if (typeof (savedObject) == 'number') $(context).find("#Id").val(savedObject);
                    else if (savedObject.Id) $(context).find("#Id").val(savedObject.Id);
                    else if (savedObject.Id == 0) {
                        $("[data-role='multiselect']").each(function () {$(this).data("kendoMultiSelect").value([])});
                        $(context)[0].reset();
                        for (var i in savedObject) {
                            if (savedObject.hasOwnProperty(i)) {
                                var f = $(context).find("#" + i);
                                if (f) (savedObject[i] ? f.val(null) : f.val(savedObject[i]));
                            }
                        }
                    }
                }
                afterSave(savedObject);
            }
            else {
                toastr.success(allMsgs.SaveComplete);
                updateHistory(oldUlr);
            }
        }
            ///Array of errors
        else if (Array.isArray(data)) {
            closeWindow();

            var msg = "";
            for (var i in data) {
                msg += data[i].Message + " ";
                if (data[i].Field) {
                    var field2 = $(context).find("#" + data[i].Field);
                    if (field2 && !field2.hasClass("k-invalid")) {
                        field2.addClass("k-invalid").prop("aria-invalid", true);
                        field2.after("<span class='k-widget k-tooltip k-tooltip-validation k-invalid-msg' data-for='" + data[i].Field + "' role='alert'>" + data[i].Message + "</span>");
                    }
                }
            }

            $(".save-group").find("button").attr('disabled', false);
            $(context).find('form').attr('data-disable', true);
            $("#b6,#b7").attr('disabled', true);
            var errorbox = $('#errorbox');
            if (errorbox.length == 0) toastr.error(msg); else { errorbox.css('display', '').text(msg); $(".alert").css("display", ""); };
        }
            ///errors from DB
        else {
            closeWindow();
            $(".save-group").find("button").attr('disabled', false);
            $(context).find('form').attr('data-disable', true);
            $("#b6,#b7").attr('disabled', true);
            var errorbox = $('#errorbox');
            if (errorbox.length == 0) toastr.error(msg); else errorbox.css('display','').text(msg);
        }

        function closeWindow() { //for company
            var waitWindow = $("#waitWindow").data("kendoWindow");
            if (waitWindow) waitWindow.close();
        }
    }

    //#Validation region 
    (function ($, kendo) {
        $.extend(true, kendo.ui.validator, {
            rules: {
                "disable-weekend": function (input) {
                    var field2;

                    if (input.is("[data-role=datepicker]")) field2 = input.data("kendoDatePicker");
                    else if (input.is("[data-role=datetimepicker]")) field2 = input.data("kendoDateTimePicker");

                    if (field2)
                        return !(field2.value() == null && input.val()); //disabled date and not empty

                    return true;
                },
                formlength: function (input) {
                    var min = input.attr("formlength");
                    if (input && input.val() && input.val().length < min) {
                        return false;
                    }
                    return true;
                },
                formreq: function (input) {
                    var flag = false;
                    var type = input.attr("type");
                    var form = input.parents("form");
                    if (form.filter("[type='radio']") && type == "radio" && input.attr("formreq"))
                        return form.find("[type='radio']").is(':checked');

                    if (input.attr("formreq") == "formreq") {

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
                isunique: function (input) {
                    var attrbutes = "";
                    var flag2 = true;

                    var validate = input.attr('isunique');
                    input.data('availableUrl', '/Pages/IsUniqueP');
                    if (typeof validate !== 'undefined' && validate !== false) {
                        var settings = {
                            url: input.data('availableUrl') || '/Pages/IsUnique',
                        };
                        var res = availability.check(input, settings)
                        if (res == true) {
                            return true;
                        } else
                            return false;
                    }
                    return flag2;

                }
            },
            messages: {
                "disable-weekend": function () {
                    return allMsgs.diabledDate ? allMsgs.diabledDate : "This date is disabled";
                },
                datepicker: function (input) {
                    return input.val() + " " + allMsgs.invalidDate ? allMsgs.invalidDate : "Invalid Date";
                },
                formlength: function (input) {
                    return (allMsgs.LengthCantBeLessThan ? allMsgs.LengthCantBeLessThan : "Text length can't be less than") + " " + input.attr("formlength");
                },
                pattern: function (input) {
                    var name = input.attr('name');
                    return name + " " + (allMsgs.IsNotMatchPattern ? allMsgs.IsNotMatchPattern : "Input is not matched pattern");
                },
                isunique: function (input) {
                    var value = input.val();
                    return value + " " + (allMsgs.AlreadyExists ? allMsgs.AlreadyExists : "Entered data already exists");
                },
                formreq: function (input) {
                    return (allMsgs.Required ? allMsgs.Required : "This field is required");
                }
            },
            errorTemplate: "<span style='color:red;'>#=message#</span>"

        })

    })(jQuery, kendo);

    var availability = {
        check: function (element, setting) {
            var flag = true;
            var id = element.attr('id');

            $.ajax({
                url: setting.url,
                type: "POST",
                dataType: 'json',
                async: false,
                data: validationData(element),
                contentType: "application/json",
                success: function (data) {
                    if (data == false) {
                        flag = false;
                    }
                },
                error: function (error) {

                }
            });
            return flag;
        }
    }

    function validationData(element) {

        var model = {}, formDiv = element.closest("form"),
            columnsData = {};
        //[0] for js object
        getColumnData(element[0], columnsData);
        var value = columnsData[element.attr("name")];

        model.id = element.parents("form").find("#Id").val();

        ///not in FlexData
        if (element.closest('.set.flex-columns').length == 0) {
            model.tablename = formDiv.attr("tableName");
            if (model.tablename === undefined)
                model.tablename = $(element).closest("div.modal-body").attr("tablename");

            model.values = [value];
            model.columns = [element.attr("name")];

            model.IsLocal = formDiv.attr("IsLocal");
            model.parentId = formDiv.attr("parentId");
            model.parentColumn = formDiv.attr("parentColumn");
        }
        else {
            ///FlexData
            model.id = element.attr("flexId");
            model.tablename = 'FlexData';
            model.columns = ['ColumnName', 'TableName', 'Value', 'PageId'];
            model.values = [element.attr('name'), element.attr("tableName"), value, element.attr('pageId')];
        }

        if (element.attr("uniquecolumns")) {
            var uniqueCols = element.attr("uniquecolumns").split(',');

            for (var i = 0; i < uniqueCols.length; i++) {
                var columnsData = {}, field2 = $('#' + uniqueCols[i]);
                if (field2.length) {
                    getColumnData(field2[0], columnsData);

                    model.columns.push(uniqueCols[i]);
                    model.values.push(columnsData[uniqueCols[i]]);
                }
            }
        }

        return JSON.stringify(model);
    }
    //#end validation region


    function addFormError(formId, fieldName, message) {
        var field2 = $("#" + formId + " #" + fieldName);
        if (field2 && !field2.hasClass("k-invalid")) {
            field2.addClass("k-invalid").prop("aria-invalid", true);
            field2.after("<span class='k-widget k-tooltip k-tooltip-validation k-invalid-msg' data-for='" + fieldName + "' role='alert'>" + message + "</span>");
        }
    }

    function removeFormError(formId, fieldName) {
        var field2 = $("#" + formId + " #" + fieldName);
        field2.removeClass("k-invalid").prop("aria-invalid", false);
        field2.next("span.k-tooltip-validation").remove();
    }

    function disbaleKendoDates(field2, disableDatesFunc) {
        var container = $(field2).closest("div"),
            culture = $(field2).closest('form').attr('culture');

        for (var i = 0; i < field2.length; i++) {
            var options = { culture: culture, disableDates: disableDatesFunc, parseFormats: ["yyyy-MM-dd"] }; ///$(field[i]).data(picker).options.disableDates
            if (field2[i].attributes.value) $(field2[i]).val(kendo.toString(field2[i].attributes.value.nodeValue, "yyyy-MM-dd"));

            $(field2[i]).removeAttr("data-role");
            $(container[i]).empty();
            $(container[i]).append(field2[i]);

            $(field2[i]).kendoDatePicker(options);
        }
    }

    var deletedColumnsIds = [], deletedSetsIds = [];
    function removeFeild(target) {
        //e.preventDefault();
        var element = $(target).closest(".form-group");
        if (element.length == 0) element = $(target).closest(".set");
        var section = $(element).closest(".section");
        element.remove();

        var id;
        if (element.hasClass("set")) {
            id = element.attr("id");
            deletedSetsIds.push(id);
        }
        else {
            if (section.children().length == 0) section.remove();
            id = element.find("input, select, textarea").attr("id");
            deletedColumnsIds.push(id);
        }

    }

    ///#region Save Design

    ///--Editables & Orders
    ///Editables : colName, objectName, culture, Title
    ///Orders(Feilds & Section) : Id, Order
    function saveFormDesign(context) {
        var data = {};
        data.ColumnTitles = [];
        data.FieldSets = [];
        data.Sections = [];
        data.DeletedColumnsIds = deletedColumnsIds;
        data.DeletedSetsIds = deletedSetsIds;
        var myForm = $(context).find("form");
        data.ObjectName = myForm.attr("objectName");
        data.TableName = myForm.attr("tableName");
        data.Culture = myForm.attr("culture");
        data.Title = myForm.attr("formTitle");
        data.Version = myForm.attr("version");

        ///--Order
        var fieldsets = $(context).find(".set");
        for (var i = 0; i < fieldsets.length; i++) {
            //FieldSets Order
            var fsInfo = {};
            fsInfo.Order = i + 1;
            fsInfo.Id = fieldsets[i].id;
            data.FieldSets.push(fsInfo);

            //Sections Order
            var sections = $(context).find(".set:eq(" + i + ")").children(".sections").children();
            for (var j = 0; j < sections.length; j++) {
                var secInfo = {};
                secInfo.Order = j + 1;
                secInfo.Id = sections[j].id;
                if (secInfo.Id != "undefined")
                    data.Sections.push(secInfo);
            }
        }

        ///--Labels
        var editables = $(context).find(".lblEditable").find(".lblSpan").parent().not(".radio, :hidden").add($(context).find("#titleLbl"));
        for (var i = 0; i < editables.length; i++) {
            var label = {};
            label.ObjectName = $(context).find("form").attr("objectName");
            label.Culture = $(context).find("form").attr("culture");
            if ($(editables[i]).attr("for")) //column
                label.ColumnName = editables[i].htmlFor;
            else if ($(editables[i]).hasClass("set-title") || $(editables[i]).hasClass("o-collabsebtn")) //set
                label.ColumnName = $(editables[i]).closest(".set").attr("name");
            else //form title
                label.ColumnName = $(editables[i]).closest("#titleLbl").attr("name");
            
            label.Title = editables[i].innerText;
            data.ColumnTitles.push(label);
        }

        // Save to server
        $.ajax({
            url: "/Pages/SaveForm",
            type: 'POST',
            data: { form: data },
            dataType: 'json',
            success: function (result) {
                if (result == "OK")
                    toastr.success(allMsgs.SaveDesignSuccess);
                else
                    toastr.error(result);
            },
            error: function (result) {
                toastr.error(result.responseText);
            }
        });
    }
    ///#endregion Save Design

    //Save Window Size
    function saveWindowSize(windowId, formName) {
        var myWindow = $("#" + windowId).closest(".k-window");
        var myWindowSize = { width: myWindow.css("width"), height: myWindow.css("height") };
        localStorage.setItem("window-" + windowId + "-" + formName, JSON.stringify(myWindowSize));
    }

    $.urlParam = function (name) {
        if (ulr !== undefined) {
            var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(ulr);
            if (results) return results[1] || 0; // decodeURI(results[1]) for special char
        }
        return null;
    }

    ///#region Tabs
    //container: div --- tabs(optional): [{Name, Title}]
    function DrawTabs(container, tabs) {
        var it;
        if (tabs) it = tabs;
        else {
            var role = $.urlParam('RoleId');
            var menu = $.urlParam('MenuId');
            it = $.grep(JSON.parse(localStorage["Tabs"]), function (e) { return e.RoleId == role && e.ParentId == menu; });
        }

        var menuHtml = '<div class="portlet box"><div class="portlet-body">';
        menuHtml += '<div id="classrow">';
        menuHtml += '<div id="classcol-md-2">'
        menuHtml += '<div class="tab-tools">';
        menuHtml += '</div>'
        menuHtml += '<ul class="nav nav-tabs" id="menu_tabs">';
        var content = '<div id="classcol-md-10"><div class="tab-content">';

        for (var i = 0 ; i < it.length ; i++) {
            menuHtml += '<li><a id="t_' + it[i].Name + '" href="#tab_' + it[i].Name + '" data-menuId="' + it[i].Id + '" data-toggle="tab">'
                + (it[i].Class ? '<span class="' + it[i].Class + '"></span> ' : '') + it[i].Title + '</a></li>';
            content += '<div class="tab-pane fade" id="tab_' + it[i].Name + '"></div>';
        }

        menuHtml += '</ul></div>';
        //content += '';

        //pageTabs
        $("#" + container).prepend(menuHtml + content + '</div></div></div></div>');
        $('a[href*="tab_"]').first().parent().addClass('active');
        $('[id*="tab_"]').first().addClass('active in');
    }
    ///#endregion Tabs

    function parseServerDate(date) {
        if (date && date.indexOf('/Date') != -1) return kendo.toString(new Date(parseInt(date.substr(6))), "yyyy-MM-dd"); //.toLocaleDateString();
        else return date;
    }

    return {
        DrawTabs: DrawTabs,
        fillOptionsDynamic: fillOptionsDynamic,
        saveForm: saveForm,
        postSuccessFunc: postSuccessFunc,
        parseServerDate: parseServerDate,
        getColumnData: getColumnData,
        saveWindowSize: saveWindowSize,
        field: field,
        addFormError: addFormError,
        removeFormError: removeFormError,
        disbaleKendoDates: disbaleKendoDates
    }
}();