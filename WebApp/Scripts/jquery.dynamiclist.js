/// <reference path="../jquery-2.1.4.js" />
/// <reference path="../kendo/2014.1.416/kendo.web.min.js" />
/// <reference path="jquery.validate.unobtrusive.dynamic.js" />

(function(jQuery)
{
    var privateMethods = {
        addItem: function(e, options)
        {
            if (e)
            {
                e.preventDefault();
            }

            options.addButton.prop("disabled", true);

            // build the html field prefix
            var index = privateMethods.getNewIndex(options.list, options.itemSelector);
            var htmlFieldPrefix = privateMethods.getNewItemPrefix(options.htmlFieldPrefix, options.property, index);
            // append the new item via ajax
            var getUrl = typeof (options.newItemUrl) === "function" ? options.newItemUrl() : options.newItemUrl;
            if (getUrl.indexOf("?") < 0)
            {
                getUrl += "?";
            }
            jQuery.get(getUrl + "&htmlFieldPrefix=" + htmlFieldPrefix, function(html)
            {
                if (options.listType === "table")
                {
                    options.list.find("tbody").append(html);
                    options.list.find(options.itemSelector + ":last").append("<td class='delete-cell'>");
                }
                else
                {
                    var $lastItem = options.list.find(options.itemSelector + ":last");
                    if ($lastItem.length > 0)
                    {
                        $lastItem.after(html);
                    }
                    else
                    {
                        options.list.prepend(html);
                    }
                }

                var $item = jQuery(options.itemSelector + ":last", options.list);
                var indexName = privateMethods.getIndexName(options.htmlFieldPrefix, options.property);
                privateMethods.appendRemoveAndIndex($item, index, indexName, options);

                if (jQuery.validator && jQuery.validator.unobtrusive && jQuery.validator.unobtrusive.parseDynamicContent)
                {
                    jQuery.validator.unobtrusive.parseDynamicContent($item);
                }

                if (options.itemAdded)
                {
                    options.itemAdded($item);
                }

                options.addButton.prop("disabled", false);
            }).fail(function (xhr)
            {
                if (xhr.status === 401)
                {
                    // let the user know that their session timed out.
                    var message = "Your session has timed out. Please refresh the page and try again.";
                    if (console && console.log)
                    {
                        console.log(message);
                    }
                    else
                    {
                        alert(message);
                    }
                }
            });
        },
        removeItem: function(item, options)
        {
            if (options.removeConfirmation && !confirm(options.removeConfirmation)) {
                return;
            }

            var $item = $(item);

            if (options.itemRemoved) {
                options.itemRemoved($item);
            }

            $item.remove();
        },
        appendRemoveAndIndex: function($item, index, indexName, options)
        {
            /// <summary>Appends the remove button and hidden field for the item index.</summary>

            var buttonHtml = privateMethods.parseTemplate("removeItem", options);

            // This hidden index field is required so that we can dynamically remove items from the middle of the list.
            // If we didn't have this, we'd have to use sequential indexes and MVC would only bind some values.
            // See http://haacked.com/archive/2008/10/23/model-binding-to-a-list.aspx
            var hiddenHtml = '<input type="hidden" name="' + indexName + '" class="index" value="' + index + '" />';
            var buttonAndHiddenHtml = buttonHtml + hiddenHtml;

            if (options.listType === "table")
            {
                $item.find("td.delete-cell").append(buttonAndHiddenHtml);
            }
            else
            {
                $item.append(buttonAndHiddenHtml);
            }

            // delete button handler
            // IE7 has a problem with using this with live(), other wise I would use that instead
            jQuery(".delete-item", $item).click(function(e)
            {
                e.preventDefault();
                privateMethods.removeItem($item, options);
            });
        },
        getBaseItemPrefix: function(baseHtmlFieldPrefix, property)
        {
            // build the html field prefix
            var prefix = baseHtmlFieldPrefix;
            if (prefix !== "")
            {
                prefix += ".";
            }
            prefix += property;

            return prefix;
        },
        getNewItemPrefix: function(baseHtmlFieldPrefix, property, index)
        {
            return privateMethods.getBaseItemPrefix(baseHtmlFieldPrefix, property) + "[" + index + "]";
        },
        getIndexName: function(baseHtmlFieldPrefix, property)
        {
            return privateMethods.getBaseItemPrefix(baseHtmlFieldPrefix, property) + ".Index";
        },
        getNewIndex: function($list, itemSelector)
        {
            /// <summary>Returns a unique index whose value is greater than any existing index.</summary>

            var $items = jQuery(itemSelector, $list);
            if ($items.length === 0)
            {
                return 0;
            }

            var maxIndex = 0;
            $items.each(function()
            {
                var index = parseInt(jQuery("input.index", jQuery(this)).val());
                if (isNaN(index))
                {
                    index = 0;
                }

                maxIndex = Math.max(maxIndex, index);
            });

            return maxIndex + 1;
        },
        getTemplate: function(template, options)
        {
            return options.templates[options.listType][template];
        },
        parseTemplate: function(template, options)
        {
            var html = privateMethods.getTemplate(template, options);
            Object.keys(options).forEach(function(key)
            {
                html = html.replace("{" + key + "}", options[key] || "");
            });
            return html;
        }
    };

    //
    // plugin methods
    //
    var methods = {
        init: function(options)
        {
            options = jQuery.extend({}, jQuery.fn.dynamiclist.defaults, options);

            return this.each(function()
            {
                var $list = jQuery(this);
                var data = $list.data("dynamiclist");

                options.list = $list;

                // only initialize once
                if (!data)
                {
                    $list.wrap(privateMethods.parseTemplate("container", options));
                    $list.addClass("dynamic-list");

                    options.selector = $list.selector;

                    // add the delete column
                    if (options.listType === "table")
                    {
                        jQuery("thead tr", $list).append("<th><\/th>");
                        jQuery("tbody tr", $list).append("<td class='delete-cell'><\/td>");
                        jQuery("tfoot tr", $list).append("<td><\/td>");
                    }

                    // init existing items
                    jQuery(options.itemSelector, $list).each(function(i)
                    {
                        var htmlFieldPrefix = privateMethods.getIndexName(options.htmlFieldPrefix, options.property);
                        privateMethods.appendRemoveAndIndex(jQuery(this), i, htmlFieldPrefix, options);
                    });

                    // add the add button
                    var addItemHtml = privateMethods.parseTemplate("addItem", options);
                    if (options.listType === "table")
                    {
                        var colspan = $list.find("thead td, thead th").length;
                        var $tfoot = $list.find("tfoot");
                        if ($tfoot.length === 0)
                        {
                            $list.append("<tfoot><\/tfoot>");
                            $tfoot = $list.find("tfoot");
                        }
                        $tfoot.append('<tr><td colspan="' + colspan + '">' + addItemHtml + "<\/td><\/tr>");
                    }
                    else
                    {
                        $list.append(addItemHtml);
                    }

                    var $button = options.addButton = $list.find(".add-item");
                    $button.click(function(e)
                    {
                        privateMethods.addItem(e, options);
                    });

                    // save options
                    $list.data("dynamiclist", options);

                    if (options.itemAdded)
                    {
                        $(options.itemSelector, $list).each(function(i, item)
                        {
                            options.itemAdded(item);
                        });
                    }
                }
            });
        },
        options: function(options)
        {
            return this.each(function()
            {
                var $list = jQuery(this);
                var currentOptions = $list.data("dynamiclist") || {};
                options = jQuery.extend({}, currentOptions, options);
                $list.dynamiclist("destroy").dynamiclist("init", options);
            });
        },
        add: function()
        {
            return this.each(function()
            {
                var $list = jQuery(this);
                privateMethods.addItem(null, $list.data("dynamiclist"));
            });
        },
        remove: function(item)
        {
            var $item = $(item);
            var $wrapper = $(item).parents(".dynamic-list");
            var options = $wrapper.data("dynamiclist");
            privateMethods.removeItem($item, options);
        },
        destroy: function()
        {
            return this.each(function()
            {
                var $list = jQuery(this);

                //TODO: restore items to original state

                $list.removeData("dynamiclist");
            });
        }
    };

    jQuery.fn.dynamiclist = function(method)
    {
        /// <summary>Creates the necessary elements for a list of items with support for adding and removing items.</summary>

        if (methods[method])
        {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        }
        else if (typeof(method) === "object" || !method)
        {
            return methods.init.apply(this, arguments);
        }
        else
        {
            jQuery.error("Method " + method + " does not exist on jQuery.dynamiclist");
        }
    };

    //
    // plugin defaults
    //
    jQuery.fn.dynamiclist.defaults = {
        // Selector for each item in the list
        itemSelector: ".item",
        // Label for the add button
        addLabel: "Add",
        // Label for the remove button
        removeLabel: "Remove",
        // ASP.NET MVC html field prefix
        htmlFieldPrefix: "",
        // Model property that contains this list. Each item input is assumed to have a name of HtmlFieldPrefix.Property[index].BindingProperty
        property: "Items",
        // Action url for the new item partial view. This can be a string or function. The url should accept a htmlFieldPrefix parameter. E.g. Controller/Action?htmlFieldPrefix=Model.Property
        newItemUrl: "NewItem",
        // list, table, or div
        listType: "list",
        // Occurs after an item is added to the list
        itemAdded: function(item) { },
        // Occurs after an item is removed from the list
        itemRemoved: function (item) { },
        // If defined, will display a confirmation dialog with the given message.
        removeConfirmation: null,
        // define custom templates if you need more control over the styling
        templates : {
            table: {
                container: "<div class='dynamic-list-container'><\/div>",
                addItem: "<div class=\"add-item-container\"><button class=\"add-item\">{addLabel}<\/button><\/div>",
                removeItem: "<button type=\"button\" class=\"delete-item\">{removeLabel}</button>"
            },
            list: {
                container: "<div class='dynamic-list-container'><\/div>",
                addItem: "<li class=\"add-item-container\"><button class=\"add-item\">{addLabel}<\/button><\/li>",
                removeItem: "<button type=\"button\" class=\"delete-item\">{removeLabel}</button>"
            },
            div: {
                container: "<div class='dynamic-list-container'><\/div>",
                addItem: "<div class=\"add-item-container\"><button class=\"add-item\">{addLabel}<\/button><\/div>",
                removeItem: "<button type=\"button\" class=\"delete-item\">{removeLabel}</button>"
            }
        }
    };
})(jQuery);