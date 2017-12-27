function dragOver(treeview, node) {
    var childNodes = $(".k-item", node);
    var order = 0;
    var id = treeview.dataItem(node).Id;
    if (childNodes && childNodes.length) {
        for (var i = 0; i < childNodes.length; i++) {
            var child = treeview.dataItem(childNodes[i]);
            if (id == child.ParentId)
                order = child.Order;
        }
    }
    return order + 100;
}

function newOrder(treeview, node, oldorder, before) {
    var childNodes = $(".k-item", node.parent());
    var order = 100;
    if (childNodes && childNodes.length) {
        var lastnode = false;
        var orders = [], previous = oldorder;
        for (var i = 0; i < childNodes.length; i++) {
            var child = treeview.dataItem(childNodes[i]);
            orders.push(child.Order);
            if (oldorder === 0 && child.Order !== 0) previous = child.Order; // append nodes only
            lastnode = child.Order === 0; // insert nodes
        }

        if (before === true) // drag before
            order = Number(previous) - 1;
        else
            order = (lastnode === true ? Number(previous) + 100 : Number(previous) + 1); // append to last

        //prevent duplicate key
        while ($.inArray(order, orders) >= 0) {
            order = (before === true ? Number(order) - 1 : Number(order) + 1);
        }
    }
    return order;
}

function addNode(selector, source) {
    var treeview = $(selector).data("kendoTreeView");
    var selected = treeview.select();
    selected.find("span.k-state-selected").removeClass("k-state-selected");
    $("#PopupMenu").remove();

    var model = treeview.dataItem(selected);
    var node = treeview.findByUid(model.uid);
    var json = model.toJSON();

    if (source) { // copy node
        json.Name = source.Name;
        json.Title = source.Title;
        json.Icon = source.Icon;
    } else {
        json.Icon = "glyphicon glyphicon-star-empty";
        json.Name = "NewNode";
        json.Title = msg[0];
    }

    json.Id = 0;
    json.Order = 0;
    var newmodel = treeview.dataItem(treeview.insertAfter(json, node));

    // update Order, Sort
    newmodel.Order = newOrder(treeview, node, model.Order);

    //set sort
    var pad = "00000";
    var sort = model.Sort.length > 5 ? model.Sort.substring(0, model.Sort.length - 5) : "";
    newmodel.Sort = sort + pad.substring(0, 5 - String(newmodel.Order).length) + String(newmodel.Order);
    return newmodel;
}

function copy(selector) {
    var treeview = $(selector).data("kendoTreeView");
    copyModel = treeview.dataItem(treeview.select());
}

function pasteChild(selector, table, copypages) {
    var treeview = $(selector).data("kendoTreeView");
    var json = copyModel.toJSON();
    addChild(selector, json, table, copypages != undefined);
}

function paste(selector, table, copypages) {
    var treeview = $(selector).data("kendoTreeView");
    var json = copyModel.toJSON();
    var dest = addNode(selector, json);
    send(treeview, table, json, dest, copypages != undefined);
}

function send(treeview, table, json, dest, copypages) {
    json.Order = dest.Order;
    json.Sort = dest.Sort;
    $.ajax({
        url: "../Pages/DropMenuItemCopy",
        type: 'POST',
        dataType: 'json',
        data: JSON.stringify({ model: json, tablename: table, copypages: copypages }),
        contentType: 'application/json',
        success: function (result) {
            if (result.substr(0, 2) != "OK") {
                toastr.error(result);
                return;
            }

            dest.Id = result.substr(3); 
            treeview.select(null);
            copyModel = null;
        },
        error: function (result) {
            console.log(result);
        }
    });
}

var msg, copyModel = null;

function addChild(selector, source, table, copypages) {
    var treeview = $(selector).data("kendoTreeView");
    var selected = treeview.select();
   
    selected.find("span.k-state-selected").removeClass("k-state-selected");
    $("#PopupMenu").remove();
    var model = treeview.dataItem(selected);
    if (treeview.beforeInsertChild && treeview.beforeInsertChild(model) === false) return;
   

    var node = treeview.findByUid(model.uid);
    var json = model.toJSON();

    if (source) { // copy node
        json.Title = source.Title;
        json.Name = source.Name;
        json.Icon = source.Icon;
    } else {
        json.Icon = "glyphicon glyphicon-star-empty";
        json.Name = "NewNode";
        json.Title = msg[0];
    }

    json.Id = 0;
    json.Order = 0;
    json.ParentId = model.Id;
    json.ParentName = model.Name;
   
   
    treeview.append(json, node, function () {
        var newmodel = treeview.dataItem($(node).find("li.k-item.k-last"));
        node = treeview.findByUid(newmodel.uid);

        // update ParentId, Order, Sort
        newmodel.Order = newOrder(treeview, node, 0);
       
        //set sort
        var pad = "00000";
        newmodel.Sort = model.Sort + pad.substring(0, 5 - String(newmodel.Order).length) + String(newmodel.Order);
        
        if (table) {
            source.ParentId = json.ParentId;
            source.ParentName = json.ParentName;
            send(treeview, table, source, newmodel, copypages);
        }
    });
}

function remove(selector) {
    var treeview = $(selector).data("kendoTreeView");
    var model = treeview.dataItem(treeview.select());
    var node = treeview.findByUid(model.uid);
    treeview.remove(node);
}

function bindChecked(Nodes, selectedDepts) {
    for (var i = 0; i < Nodes.length; i++) {
        Nodes[i].set("checked", $.inArray(Nodes[i].Id, selectedDepts) != -1);
        if (Nodes[i].hasChildren) bindChecked(Nodes[i].children.view(), selectedDepts);
    }
}

function checkNodeIds(Nodes, selectedDepts) {
    for (var i = 0; i < Nodes.length; i++) {
        if (Nodes[i].checked)
            selectedDepts.push(Nodes[i].Id);

        if (Nodes[i].hasChildren) checkNodeIds(Nodes[i].children.view(), selectedDepts);
    }
}


jQuery.fn.extend({
    TreeView: function (parm) {
        var dataSource = new kendo.data.HierarchicalDataSource({
            transport: {
                read: {
                    url: "/Pages/GetTree?table=" + parm.table,
                    dataType: "json"
                }
            },
            schema: {
                model: {
                    id: "Id",
                    icon: "Icon"
                }
            }
            
        });

        var selector = "'" + $(this).selector + "'";
        //if ($.inArray(parm.lang.split('-')[0], ["ps", "ar", "ur", "ku", "fa"]) >= 0) $("body").addClass("k-rtl");

        $(this).kendoTreeView({
            loadOnDemand: parm.loadOnDemand || true,
            dragAndDrop: parm.dragAndDrop,
            dataSource: dataSource,
            dataTextField: "Title",
            dataSpriteCssClassField: "Icon",
            checkboxes: parm.checkboxes || false,
            check: function(e) {
                if (parm.check) parm.check(e);
            },
            select: function (e) {
                if (parm.showMenu && parm.showMenu === true) {
                    $("#PopupMenu").remove();
                    var n = $(e.node).find("span.k-in");
                    $(n).addClass("k-state-selected");
                    // CUT <li><a onclick="cut()"><i class="fa fa-scissors" ></i><span>&nbsp; ' + msg[5] + '</span></a></li>
                    var menus = parm.table === "Menus" ? '<li class="divider"></li><li><a onclick="paste(' + selector + ",'" + parm.table + "',true" + ')"><i class="fa fa-file-powerpoint-o" ></i><span>&nbsp; ' + msg[10] + '</span></a></li><li><a onclick="pasteChild(' + selector + ",'" + parm.table + "',true" + ')"><i class="fa fa-file-code-o" ></i><span>&nbsp; ' + msg[11] + '</span></a></li>' : '';
                    var hide = copyModel == null ? 'style="display: none"' : '';
                    $(n).first().after('<div id="PopupMenu" class="btn-group"><button type="button" class="btn btn-icon-only glyphicon glyphicon-option-horizontal dropdown-toggle small" style="height: 20px; width: 30px; padding: 0" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" style="padding: 0"></button><ul class="dropdown-menu"><li><a onclick="addNode(' + selector + ')"><i  class="fa fa-plus"></i><span>&nbsp; ' + msg[1] + '</span></a></li><li><a onclick="addChild(' + selector + ')"><i class="fa fa-child"></i><span>&nbsp; ' + msg[2] + '</span></a></li><li><a><i class="fa fa-edit"></i><span>&nbsp; ' + msg[3] + '</span></a></li><li><a><i class="fa fa-remove"></i><span>&nbsp; ' + msg[4] + '</span></a></li><li class="divider"></li><li><a  onclick="copy(' + selector + ')"><i class="fa fa-copy" ></i><span>&nbsp; ' + msg[6] + '</span></a></li><li class="dropdown-submenu pull-left" ' + hide + '><a tabindex="-1"><i class="fa fa-clipboard"></i><span>&nbsp; ' + msg[7] + '</span></a><ul class="dropdown-menu" style="left: -50%; text-align: -webkit-auto"><li><a onclick="paste(' + selector + ",'" + parm.table + "'" + ')"><i class="fa fa-file-o" ></i><span>&nbsp; ' + msg[8] + '</span></a></li><li><a onclick="pasteChild(' + selector + ",'" + parm.table + "'" + ')"><i class="fa fa-user-plus" ></i><span>&nbsp; ' + msg[9] + '</span></a></li>' + menus + '</ul></li></ul></div>');
                }

                if (parm.remove) $("a:has(>i.fa-remove)").click(parm.remove);
                if (parm.select) parm.select(e);
            },
            dataBound: function (e) {
                var li = this.select();
                var model = li && this.dataItem(li);
                if (model && model.Id === 0 && parm.add) parm.add(model);
                if (msg === undefined) msg = this.dataItem(this.items(0)).Msg;
                if (parm.dataBound) parm.dataBound(e);
            },
            drop: function (e) {
                if (e.valid === false) return;
                
                // validation section
                if (parm.beforeDrop != undefined) {
                    if (parm.beforeDrop(e, this) === false)
                        return;
                }
                var source = this.dataItem(e.sourceNode);
                var dest = this.dataItem(e.dropTarget);
                if (dest === undefined) return;
                var d = {}, pad = "00000", sort;
                if (e.dropPosition == "over") {
                   
                        d.ParentId = dest.Id;
                        d.Order = dragOver(this, this.findByUid(dest.uid));
                        d.Sort = dest.Sort + pad.substr(0, 5 - String(d.Order).length) + String(d.Order);
                   }else {
                    d.ParentId = dest.ParentId;
                    d.Order = newOrder(this, this.findByUid(dest.uid), dest.Order, e.dropPosition == "before");
                    sort = dest.Sort.length > 5 ? dest.Sort.substr(0, dest.Sort.length - 5) : "";
                    d.Sort = sort + pad.substr(0, 5 - String(d.Order).length) + String(d.Order);
                }

                var s = {};
                s.Id = source.Id;
                s.Sort = source.Sort;
                s.CompanyId = source.CompanyId;
                s.hasChildren = source.hasChildren;

                // Refresh
                source.ParentId = d.ParentId;
                source.Order = d.Order;
                source.Sort = d.Sort;

                $.ajax({
                    url: "../Pages/DropMenuItem",
                    type: 'POST',
                    dataType: 'json',
                    data: JSON.stringify({ source: s, dest: d, tablename: parm.table }),
                    contentType: 'application/json',
                    success: function (result) {
                        if (result != "OK") toastr.error(result);
                    },
                    error: function (result) {
                        console.log(result);
                    }
                });
            }
        });
    }
});
