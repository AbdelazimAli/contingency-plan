$(document).ready(function () {
    registerPleaseWaitMessage();
});

$(window).bind('load',function(){
    //$('.loader-overlay').fadeOut();
});

function registerPleaseWaitMessage() {

    $(document).ajaxSend(function (evt, request, settings) {

        //if (!settings.url.includes('ReadRemoteList'))
        //    AjaxStart();
    });

    $(document).ajaxComplete(function (event, request, settings) {
        //AjaxComplete();
    });
}

function AjaxStart() {
    //$('.MyButtons').attr('disabled', 'disabled');
    ////$('.isloading-overlay').remove();
    //// $.isLoading({ text: "Loading" });

    //$('.loader-overlay').fadeIn();

    //setTimeout(function () { $('.loader-overlay').fadeOut(); }, 2000);
}


function AjaxComplete() {
    ////$(".modal-backdrop").remove();
    //$('.MyButtons').removeAttr('disabled');
    //$.isLoading("hide");
    //if ($('.nav-tabs').children('li').length==0) {
    //    $('.nav-tabs').remove();
    //}
    //$('.loader-overlay').fadeOut();
}



function do_AjaxCall(url, data, success) {
    $.ajax({
        type: "POST",
        url: url,// "CreateMemo.aspx/GetNumberOfMemos",
        data: data,// '{"_EmpID":' + _EmpID + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        cache: false,
        processData: false,
        success: function (msg) {
            success(msg);

        }

    });
}

function do_AjaxCall_ForUploadFil(url, data, success) {
    $.ajax({
        type: "POST",
        url: url,// "CreateMemo.aspx/GetNumberOfMemos",
        data: data,// '{"_EmpID":' + _EmpID + '}',
        contentType: false,// "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        cache: false,
        processData: false,
        success: function (msg) {
            success(msg);

        }

    });
}

function do_AjaxCall_ReturnHTML(url, data, success) {
    $.ajax({
        type: "POST",
        url: url,// "CreateMemo.aspx/GetNumberOfMemos",
        data: data,// '{"_EmpID":' + _EmpID + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        async: true,
        cache: false,
        success: function (msg) {
            success(msg);

        }

    });
}

function do_AjaxCall_Before(url, data, success, beforeSend) {
    $.ajax({
        type: "POST",
        url: url,// "CreateMemo.aspx/GetNumberOfMemos",
        data: data,// '{"_EmpID":' + _EmpID + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        cache: false,
        success: function (msg) {
            success(msg);
            // $('input[type=submit],input[type=button]').removeAttr('disabled');
        },
        beforeSend: function () {
            // Handle the beforeSend event
            // $('input[type=submit],input[type=button]').attr('disabled', 'disabled');
            beforeSend();
        }
        //,
        //complete: function () {
        //    // Handle the complete event
        //    complete();
        //}

    });
}

function do_AjaxCall_WithMethod(url, data, Method, success) {
    $.ajax({
        type: Method,
        url: url,// "CreateMemo.aspx/GetNumberOfMemos",
        data: data,// '{"_EmpID":' + _EmpID + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        cache: false,
        success: function (msg) {
            success(msg);

        }

    });
}



//For File Upload (to show the selected image)
function readURL(input, ImgID, width, height) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#' + ImgID)
                .attr('src', e.target.result)
                .width(width)
                .height(height);
        };

        reader.readAsDataURL(input.files[0]);
    }
}

//*************  Upload Image Validation

function CheckFileValidation(upld_Input, spn_ErrorID, Msg_InvalidImageExt, Array_validExtensions) {

    var Files = document.getElementById(upld_Input).files;

    var ext_1 = '';
    for (var i = 0; i < Files.length; i++) {
        var SingleFile = Files[i].name;
        ext_1 = SingleFile.substring(SingleFile.lastIndexOf('.')).toLowerCase();

        var IsValidExt_1 = CheckExtention(ext_1, spn_ErrorID, Msg_InvalidImageExt, Array_validExtensions);
        if (IsValidExt_1 == false)
            return false;
    }

    return true;

    //var filePath_1 = $('#' + upld_Input).val();


    ////if (filePath_1.length < 1 ) {
    ////    alert("اختار صورة لكى تتم عملية الحفظ"); return false;
    ////}
    //if (filePath_1) {
    //    var ext_1 = '';

    //    if (filePath_1 != '')
    //        ext_1 = filePath_1.substring(filePath_1.lastIndexOf('.')).toLowerCase();




    //    var IsValidExt_1 = CheckExtention(ext_1, spn_ErrorID, Msg_InvalidImageExt, Array_validExtensions);


    //    return (IsValidExt_1);
    //}
    //else {
    //    return true;
    //}

}

function CheckExtention(ext, spnID, Msg_InvalidImageExt, Array_validExtensions) {

    var ErrorMsg = Msg_InvalidImageExt /*'امتداد الملف غير متاح'*/;

    //check if extention is valid
    if (Array_validExtensions.includes(ext)) {
        $('#' + spnID).text('');
        return true;
    }
    //for (var i = 0; i < validExtensions.length; i++) {

    //    if (ext != '' && ext == validExtensions[i]) {
    //        $('#' + spnID).text('');
    //        return true;
    //    }
    //}

    //Extention is not valid or empty
    //check if extention is empty
    if (ext == '') {
        $('#' + spnID).text('');
        return true;
    }
    else {
        $('#' + spnID).text(ErrorMsg);
        return false;
    }
}


function UpdatePeopleTrainStatus(OldStatusID, NewStatusID) {
    if (parseInt(OldStatusID) < parseInt(NewStatusID)) {
        $('div.tab-group > div > ul > li:nth-child(' + OldStatusID + ')').removeClass('current').addClass('done');
        $('div.tab-group > div > ul > li:nth-child(' + NewStatusID + ')').removeClass('disabled').addClass('active').addClass('current');
    }
    else if (parseInt(OldStatusID) > parseInt(NewStatusID)) {
        $('div.tab-group > div > ul > li:nth-child(' + OldStatusID + ')').removeClass('current').removeClass('done');
        $('div.tab-group > div > ul > li:nth-child(' + NewStatusID + ')').addClass('active').addClass('current');
    }

}


function ShowModal(myModal) {
    var myModal = $('#' + myModal);
    myModal.modal('show');
}

function HideModal(myModal) {
    var myModal = $('#' + myModal);
    myModal.modal('hide');
}


function isScriptAlreadyIncluded(src) {
    var scripts = document.getElementsByTagName("script");
    for (var i = 0; i < scripts.length; i++)
        if (scripts[i].getAttribute('src') == src) return true;
    return false;
}

function LoadGoogleMapsScripts()
{
    var src = "http://maps.googleapis.com/maps/api/js?key=AIzaSyA-qaMK41mhavSk0ng-vJ7x7vjq_toASNs";
    if (!isScriptAlreadyIncluded(src)) {
        var script = document.createElement('script');
        script.src = src;
        script.onload = function () {

            var script_2 = document.createElement('script');
            script_2.src = "/Scripts/app/MyGoogleMaps.js";
            document.head.appendChild(script_2);
        };
        document.head.appendChild(script);
    }
}