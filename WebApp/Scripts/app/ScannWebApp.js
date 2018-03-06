var storedFiles = [];
function RegisterScannerApp() {

    if (window.ws == undefined) {
        var i = 0;

        var wsImpl = window.WebSocket || window.MozWebSocket;

        window.ws = new wsImpl('ws://localhost:8181/');
        ws.onmessage = function (e) {
            if (typeof e.data === "string") {
                //IF Received Data is String
            }
            else if (e.data instanceof ArrayBuffer) {
                //IF Received Data is ArrayBuffer
            }
            else if (e.data instanceof Blob) {
                i++;
                var f = e.data;
                f.name = "File" + i;
                var reader = new FileReader();
                reader.onload = function (e) {
                    var ImageDataURL=e.target.result.replace("data:;base64,", "data:image/jpeg;base64,");
                    $('#' + AdditionalImageElement).hide();
                    $('#' + Element_ToPreview).replaceWith('<embed id="empd_Element"  alt="pdf" >');
                    $('#' + Element_ToPreview).attr('src', ImageDataURL);
                    resize(500, 700, ImageDataURL);
                  
                }
                reader.readAsDataURL(f);
            }
        };
        ws.onopen = function () {
            //Do whatever u want when connected succesfully
        };
        ws.onclose = function () {
            $('.dalert').modal('show');
        };
    }
    
}

function scanToJpg() {
    ws.send("1100");
};



function imageDataURItoBlob(dataURI) {
    // convert base64/URLEncoded data component to raw binary data held in a string
    var byteString;

    if (dataURI.split(',')[0].indexOf('base64') >= 0)
        byteString = atob(dataURI.split(',')[1]);
    else
        byteString = unescape(dataURI.split(',')[1]);

    // separate out the mime component
    var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];

    // write the bytes of the string to a typed array
    var ia = new Uint8Array(byteString.length);

    for (var i = 0; i < byteString.length; i++) {
        ia[i] = byteString.charCodeAt(i);
    }

    return new Blob([ia], {
        type: mimeString
    });

}


function GetImageWithFormData(formId) {
    //********Scanner*********//
    var formData = formId;
    var e = document.getElementById(formId);
    formData = new FormData(e);
    for (i = 0; i < storedFiles.length; i++) {
        var img = storedFiles[i];
        formData.append('file', img, 'blob.jpg');
    }
    return formData;
}



function SubmitDataWithScannedImage(formId, inputFileID, callBack) {
    var ImageWithFormData = GetImageWithFormData(formId);

    if (!(ImageWithFormData instanceof FormData)) {
        var FormObject = document.getElementById(formId);
        ImageWithFormData = new FormData(FormObject);
    }
    var request = new XMLHttpRequest();
    request.open("POST", $('#' + formId + '').attr('action'));
    request.onload = callBack;
    request.send(ImageWithFormData);
}


function resize(/*file,*/ max_width, max_height/*, compression_ratio*/, imageEncoding) {
   // var fileLoader = new FileReader(),
    canvas = document.createElement('canvas'),
    context = null,
    imageObj = new Image(),
    imageObj.src = imageEncoding;
    blob = null;

    //create a hidden canvas object we can use to create the new resized image data
    canvas.id = "hiddenCanvas";
    canvas.width = max_width;
    canvas.height = max_height;
    canvas.style.visibility = "hidden";
    document.body.appendChild(canvas);

    //get the context to use 
    context = canvas.getContext('2d');

    // check for an image then
    //trigger the file loader to get the data from the image         
    //if (file.type.match('image.*')) {
    //    fileLoader.readAsDataURL(file);
    //} else {
    //    alert('File is not an image');
    //}

    // setup the file loader onload function
    // once the file loader has the data it passes it to the 
    // image object which, once the image has loaded, 
    // triggers the images onload function
    //fileLoader.onload = function () {
    //    var data = this.result;
    //    imageObj.src = data;
    //};

    //fileLoader.onabort = function () {
    //    alert("The upload was aborted.");
    //};

    //fileLoader.onerror = function () {
    //    alert("An error occured while reading the file.");
    //};


    // set up the images onload function which clears the hidden canvas context, 
    // draws the new image then gets the blob data from it
    imageObj.onload = function () {
        debugger;
        // Check for empty images
        if (this.width == 0 || this.height == 0) {
            alert('Image is empty');
        } else {

            context.clearRect(0, 0, max_width, max_height);
            context.drawImage(imageObj, 0, 0, this.width, this.height, 0, 0, max_width, max_height);


            //dataURItoBlob function available here:
            // http://stackoverflow.com/questions/12168909/blob-from-dataurl
            // add ')' at the end of this function SO dont allow to update it without a 6 character edit
            blob = dataURLtoBlob(canvas.toDataURL(imageEncoding));

            //pass this blob to your upload function
            storedFiles.push(blob);
        }
    };

    imageObj.onabort = function () {
        alert("Image load was aborted.");
    };

    imageObj.onerror = function () {
        alert("An error occured while loading image.");
    };

}

////**dataURL to blob**
function dataURLtoBlob(dataurl) {
    var arr = dataurl.split(','), mime = arr[0].match(/:(.*?);/)[1],
        bstr = atob(arr[1]), n = bstr.length, u8arr = new Uint8Array(n);
    while (n--) {
        u8arr[n] = bstr.charCodeAt(n);
    }
    return new Blob([u8arr], { type: mime });
}

////**blob to dataURL**
function blobToDataURL(blob, callback) {
    var a = new FileReader();
    a.onload = function (e) { callback(e.target.result); }
    a.readAsDataURL(blob);
}

//test:
//var blob = dataURLtoBlob('data:text/plain;base64,YWFhYWFhYQ==');
//blobToDataURL(blob, function (dataurl) {
//    console.log(dataurl);
//});