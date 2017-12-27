/// <reference path="jquery-2.1.3.js"/>
/// <reference path="jquery.validate.js"/>
/// <reference path="jquery.validate.unobtrusive.js"/>

(function (jQuery)
{
    // based on the example at http://xhalent.wordpress.com/2011/01/24/applying-unobtrusive-validation-to-dynamic-content/
    jQuery.validator.unobtrusive.parseDynamicContent = function (selector)
    {
        //use the normal unobstrusive.parse method
        jQuery.validator.unobtrusive.parse(selector);

        //get the relevant form
        var form = jQuery(selector).first().closest('form');

        //get the collections of unobstrusive validators, and jquery validators
        //and compare the two
        var unobtrusiveValidation = form.data('unobtrusiveValidation');
        var validator = form.validate();
        
        if (!unobtrusiveValidation || !validator)
        {
            if (console.log)
            {
                console.log("Could not find form/validator.");
            }
            return;
        }

        jQuery.each(unobtrusiveValidation.options.rules, function (elname, elrules)
        {
            if (validator.settings.rules[elname] == undefined)
            {
                var args = {};
                jQuery.extend(args, elrules);
                args.messages = unobtrusiveValidation.options.messages[elname];
                jQuery('[name="' + escapeName(elname) + '"]').rules("add", args);
            }
            else
            {
                jQuery.each(elrules, function (rulename, data)
                {
                    if (validator.settings.rules[elname][rulename] == undefined)
                    {
                        var ruleArgs = {};
                        ruleArgs[rulename] = data;
                        ruleArgs.messages = unobtrusiveValidation.options.messages[elname][rulename];
                        jQuery('[name="' + escapeName(elname) + '"]').rules("add", ruleArgs);
                    }
                });
            }
        });

        function escapeName(elname)
        {
            var escapedElname = elname;
            // We're assuming you're using html5 boilerplate style html declarations (jQuery dropped support for $.browser in 1.9):
            // <!--[if lt IE 7]> <html lang="en-us" class="no-js ie6 oldie"> <![endif]-->
            if (jQuery("html").hasClass("ie7"))
            {
                // IE7 requires that you escape . and [] in selectors - http://api.jquery.com/category/selectors/
                escapedElname = elname.toString().replace(".", "\\\\.").replace("[", "\\\\[").replace("]", "\\\\]");
            }

            return escapedElname;
        }
    };
})(jQuery);
