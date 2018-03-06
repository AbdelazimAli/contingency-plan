

/** Images scanned so far. */
imagesScanned = [];
ba = p("log_level", 0);
ea = p("form_field_name_for_img_objects", "com_asprise_scannerjs_images[]");
fa = p("form_field_name_for_img_urls", "com_asprise_scannerjs_images_urls[]");

//*********Scanner **********//

/** Initiates a scan */
function scanToJpg() {
    
        scanner.scan(displayImagesOnPage,
                {
                    "use_asprise_dialog": true,
                    "output_settings": [
                        {
                            "type": "return-base64",
                            "format": "jpg"
                        }
                    ]
                }
        );
  
}

/** Processes a ScannedImage */
function processScannedImage(scannedImage) {
    imagesScanned.push(scannedImage);

    if (AdditionalImageElement)
        $('#' + AdditionalImageElement).hide();

    debugger;
    if (Element_ToPreview) {
        $('#' + Element_ToPreview).replaceWith('<embed id="empd_Element"  alt="pdf" >');
        $('#' + Element_ToPreview).attr('src', scannedImage.src);
    }
}

/** Processes the scan result */
function displayImagesOnPage(successful, mesg, response) {
    if (!successful) { // On error
     
        console.error('Failed: ' + mesg);
        return;
    }

    if (successful && mesg != null && mesg.toLowerCase().indexOf('user cancel') >= 0) { // User cancelled.
        console.info('User cancelled');
        return;
    }

    var scannedImages = scanner.getScannedImages(response, true, false); // returns an array of ScannedImage
    for (var i = 0; (scannedImages instanceof Array) && i < scannedImages.length; i++) {
        var scannedImage = scannedImages[i];
        processScannedImage(scannedImage);

    }
}






function GetImageWithFormData(formId) {
    //********Scanner*********//
    var b = imagesScanned;
    var a = formId;
    if (b instanceof Array && 0 != b.length) {


        var e = document.getElementById(a);

        a = new FormData(e);
        for (d = 0; d < b.length; d++)
            f = b[d], f.b ? a.append(ea, aa(f.src, f.mimeType)) : a.append(fa, f.src);
        N("POST images, count: " + b.length);
    }

    return a;
}


function SubmitDataWithScannedImage(formId, inputFileID, callBack) {
    var ImageWithFormData = GetImageWithFormData(formId);

    if (!(ImageWithFormData instanceof FormData)) {
        var FormObject = document.getElementById(formId);
        ImageWithFormData = new FormData(FormObject);
    }
    //ImageWithFormData.append('Images', $('#' + inputFileID)[0].files[0]);

    var request = new XMLHttpRequest();
    request.open("POST", $('#' + formId + '').attr('action'));
    request.onload = callBack;
    request.send(ImageWithFormData);
}





function Q() {
    return "function" === typeof atob && "function" === typeof ArrayBuffer && "function" === typeof Uint8Array && "function" === typeof Blob && "function" === typeof FormData
}

function aa(a, b) {
    if (!Q()) return W("base64ToBlob() is not supported in legacy browsers."), null;
    if (null != a && 0 == a.indexOf("data:")) {
        var c = a.indexOf(";");
        !b && 0 < c && (b = a.substring(5, c));
        c = a.indexOf("base64,");
        0 < c && (a = a.substr(c + 7))
    }
    a = a.replace(/(\r\n|\n|\r)/gm, "");
    a = atob(a);
    c = a.length;
    var d = new ArrayBuffer(c);
    d = new Uint8Array(d);
    for (var f = 0; f < c; f++) d[f] = a.charCodeAt(f);
    a = new Blob([d], {
        type: b
    });
    if (!e) {
        c = new Date;
        var e = [c.getFullYear(), c.getMonth() + 1, c.getDate(), c.getHours(), c.getMinutes(), c.getSeconds()];
        e = "" + (2E3 < e[0] ? e[0] - 2E3 : e[0]) + (10 > e[1] ? "0" : "") + e[1] + (10 > e[2] ? "0" : "") + e[2] + (10 > e[3] ? "0" : "") + e[3] + (10 > e[4] ? "0" : "") + e[4] + (10 > e[5] ? "0" : "") + e[5];
        c = c.getMilliseconds();
        d = 1;
        for (f = 0; 2 > f; f++) d *= 10;
        for (d = "" + (Math.floor(Math.random() * (d - 0 + 1)) + 0) ; 2 > d.length;) d = "0" + d;
        e = e + ((100 > c ? (10 > c ? "0" : "") + "0" : "") + c) + d;
        e = e + "." + (null == b ? "unknown" : 0 <= b.toLowerCase().indexOf("bmp") ? "bmp" : 0 <= b.toLowerCase().indexOf("png") ? "png" : 0 <= b.toLowerCase().indexOf("jp") ? "jpg" : 0 <= b.toLowerCase().indexOf("tif") ? "tif" : 0 <= b.toLowerCase().indexOf("pdf") ? "pdf" : "unknown")
    }
    if (e) try {
        a.lastModifiedDate = new Date, a.name = e
    } catch (h) { }
    return a
}


function m(a, b, c, d, f) {
    this.mimeType = a;
    this.b = b;
    this.src = c;
    this.m = d;
    this.a = f
}

function N(a, b) {
    if (!(ba > (b ? 16 : 4)))
        if (window.console) {
            var c = (new Date).toLocaleTimeString();
            b ? console.error ? console.error(c + " " + a) : console.log(c + " ERROR: " + a) : console.info ? console.info(c + " " + a) : console.log(c + " INFO: " + a)
        } else b && window.alert && alert("ERROR: " + a)
}

function W(a) {
    N(a, !0)
};

function p(a, b) {
    var c = "scannerjs_" + a;
    c = window && c in window ? window[c] : void 0;
    !c && window && "scannerjs_config" in window && (c = a in window.scannerjs_config ? window.scannerjs_config[a] : void 0);
    return void 0 == c || null == c ? b : c
};
