var flex = function () {

    function flexable(container, id, childsidprefix, hoverclass, _editables, autoSavetoServer, url) {
        ///<summary>Enables reordering  of the cotainer paramter children and saves the new order to the local storage .. auto generates ids for children elements if the id doesn't exist .. the optional hoverclass adjusts the css class of the reordered elements on hover .. autosaves changes to server if autosave is true & Enable editing of labels , links , ...etc or the selector defined by the _editables paramter saves it to the local storage </summary>
        ///<param name="container" type="jquery">the html element or it's jquery selector that contains elements to reorder</param>
        ///<param name="id" type="string">[optional] the container new id if it doesn't have one </param>
        ///<param name="hoverclass" type="string">[optional] the optional hoverclass adjusts the css class of the reordered elements on hover </param>
        ///<param name="autoSavetoServer" type="boolean">the default value is false</param>
        ///<param name="url" type="string">the url for the server side save</param>
        ///<returns type=""></returns>

        enable_reorder(container, undefined, undefined, "hovercss")

        enable_reorder($(container).children("div"))

        enable_editing($(container).children("div"))
    }




    /// record.JS library depends on  JQUERY , JQUERY UI , browser Local Storage 
    function enable_reorder(container, id, childsidprefix, hoverclass, autoSavetoServer, url) {
        ///<summary>Enables reordering  of the cotainer paramter children and saves the new order to the local storage .. auto generates ids for children elements if the id doesn't exist .. the optional hoverclass adjusts the css class of the reordered elements on hover .. autosaves changes to server if autosave is true </summary>
        ///<param name="container" type="jquery">the html element or it's jquery selector that contains elements to reorder</param>
        ///<param name="id" type="string">[optional] the container new id if it doesn't have one </param>
        ///<param name="hoverclass" type="string">[optional] the optional hoverclass adjusts the css class of the reordered elements on hover </param>
        ///<param name="autoSavetoServer" type="boolean">the default value is false</param>
        ///<param name="url" type="string">the url for the server side save</param>
        ///<returns type=""></returns>
        var temp = window.location.pathname.split("/");
        // the controller name + the action name
        var uniquename = temp[1] + "_" + temp[2]

        if (id === undefined) {
            id = $(container).attr("id");
        }
        else {
            $(container).attr("id", id)
        }

        if (childsidprefix === undefined) {

            childsidprefix = "ch";
        }

        var i = 0;

        $(container).children().each(function () {

            $(this).attr("id", childsidprefix + "_" + i);
            i++;
            $(this).attr("data-toggle", "tooltip")
            $(this).attr("data-placement", "left")
            $(this).attr("title", "Drag to rearrange fields")

        })

        $('[data-toggle="tooltip"]').tooltip()
        $(container).sortable();


        if (hoverclass === undefined) {
            $(".ui-sortable-handle").hover(
             function () {
                 $(this).css("cursor", "move");
                 $(this).css({ "border-style": "dotted", "border-width": "thin" });
             }, function () {
                 $(this).css("cursor", "default");
                 $(this).css("border-style", "none")
             })
        }
        else {
            $(".ui-sortable-handle").hover(function () {
                $(this).addClass(hoverclass)
            }, function () {
                $(this).removeClass(hoverclass)
            })
        }

        $(container).on("sortupdate", function (event, ui) {
            if (typeof (Storage) !== "undefined") {
                var sorted = $(this).sortable("serialize");

                localStorage.setItem('sorted_' + uniquename + "_" + $(container).attr("id"), sorted);
            }
        });
        if (localStorage.getItem('sorted_' + uniquename + "_" + $(container).attr("id")) !== null) {
            var arrValuesForOrder = localStorage.getItem('sorted_' + uniquename + "_" + $(container).attr("id")).substring(childsidprefix.length + 3).split("&" + childsidprefix + "[]=");
            $items = $(container).children();
            for (var i = arrValuesForOrder.length ; i >= 0; i--) {
                $(container).prepend($items.get((arrValuesForOrder[i])));
            }
        }


    }

    var editable = "label,a";

    function enable_editing(container, id, _editables, autoSavetoServer) {
        /// <summary>Enable editing of labels , links , ...etc or the selector defined by the _editables paramter saves it to the local storage</summary>
        /// <param name="container" type="jquery">the html element that contains elements to edit</param>
        ///<param name="_editables" type="jquery selector">the html elements that needs to be editable the default value is "label,a"</param>
        ///<param name="autoSavetoServer" type="boolean">the default value is false</param>
        /// <returns type=""></returns>

        var temp = window.location.pathname.split("/");
        // the controller name + the action name
        var uniquename = temp[1] + "_" + temp[2]

        if (id === undefined) {
            id = $(container).attr("id");
        }
        else {
            $(container).attr("id", id)
        }

        if (_editables !== undefined) {
            editable = _editables;
        }

        if (autoSavetoServer === undefined) {
            autoSavetoServer = false;
        }

        $(container).find(editable).each(function () {
            $(this).css("display", "inline")
            $(this).attr("data-default", $(this).text())
            // console.log($(this).closest("input").first())
            //$(this).closest("input").first().attr("placeholder", $(this).text())

            //auto generate IDs if it doesn't exist
            if ($(this).attr("id") === undefined) {
                //get the first parent that has id  - H1 idea :)
                var parentid = $(this).parents("[id]").first().attr("id");
                //get the index of the current editable element
                var el_index = $(this).parents("[id]").first().find(editable).index($(this))
                $(this).attr("id", parentid + "_" + el_index)

            }
        })


        // read from local storage

        $(container).find(editable).each(function () {
            if (typeof (Storage) !== "undefined") {
                var el_id = uniquename + $(container).attr("id") + $(this).attr("id");
                var t = localStorage.getItem(el_id);

                if (t != null) {
                    $(this).text(t);
                }

                //auto detect related input

                if ($(this).attr("for") === undefined) {

                    //if it is not a label with 'for' attribute
                    $(this).next("input,textarea").first().attr("placeholder", $(this).text())

                }
                else {
                    //if it is a label with 'for' attribute... use the for attribute to locate the related input
                    $(container).find("#" + $(this).attr("for")).first().attr("placeholder", $(this).text())

                }
            }

        })

        // injecting new dom element to trigger editing
        $(container).children().hover(function () {
            $(this).find(editable).before("<button style='display:inline !important;color:black' title='Click to edit the field text'  data-toggle='tooltip' data-placement='top' onclick='flex.doedit(this)' type='button' class='btn  btn-xs glyphicon glyphicon-pencil'></button>")

        }, function () {
            $(container).find($(".glyphicon-pencil")).remove();
        })

        // attache the event handeler for writing changes to local storage on blur
        $(container).find(editable).blur(function () {

            if (typeof (Storage) !== "undefined") {
                if ($(this).text() != $(this).attr("data-default")) {

                    localStorage.setItem(uniquename + $(container).attr("id") + $(this).attr("id"), $(this).text());
                    $(this).attr("data-default", $(this).text())

                    //auto detect related input

                    if ($(this).attr("for") === undefined) {

                        //if it is not a label with 'for' attribute
                        $(this).next("input,textarea").first().attr("placeholder", $(this).text())

                    }
                    else {
                        //if it is a label with 'for' attribute... use the for attribute to locate the related input
                        $(container).find("#" + $(this).attr("for")).first().attr("placeholder", $(this).text())

                    }

                }
            }

            $(this).attr('contenteditable', 'false');

            if (autoSavetoServer == false) {
                //save to server ajax code 
            }
        });

    }




    function doedit(target) {
        var el = $(target).next(editable);
        el.attr('contenteditable', 'true');
        el.attr("spellcheck", "false")
        //var sel = window.getSelection();
        //var ran = document.createRange();
        //var element = document.getElementById($(el).attr("id"))
        //ran.selectNodeContents(element);
        //sel.addRange(ran)
        el.focus();

    }

    return {
        flexable: flexable,
        doedit: doedit,
        enable_editing: enable_editing,
        enable_reorder:enable_reorder
    }
}()