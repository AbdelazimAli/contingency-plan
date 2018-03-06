

var AllMarkers = new Array();
//$(document).ready(function () {
//    initializeGoogleMap();

//});


function initializeGoogleMap(div_map, myCenter) {

    // var myCenter = new google.maps.LatLng(30.056476,31.378389999999968);
    //create the map
    var map = CreateMap(myCenter, div_map);
    return map;
}

//Create Map ******************************//
function CreateMap(myCenter, div_map) {
    var mapProp = {
        center: myCenter,
        zoom: 12,
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        disableDefaultUI: true,

        panControl: true,
        panControlOptions: {
            position: google.maps.ControlPosition.TOP_RIGHT
        },
        zoomControl: true,
        zoomControlOptions: {
            style: google.maps.ZoomControlStyle.DEFAULT,
            position: google.maps.ControlPosition.LEFT_CENTER
        },
        mapTypeControl: true,
        mapTypeControlOptions: {
            style: google.maps.MapTypeControlStyle.HORIZONTAL_BAR,
            position: google.maps.ControlPosition.TOP_RIGHT,
            mapTypeIds: [google.maps.MapTypeId.ROADMAP, google.maps.MapTypeId.HYBRID, google.maps.MapTypeId.SATELLITE]
        },
        scaleControl: true,
        scaleControlOptions: {
            position: google.maps.ControlPosition.TOP_LEFT
        },
        streetViewControl: true,
        streetViewControlOptions: {
            position: google.maps.ControlPosition.LEFT_TOP
        },
        overviewMapControl: true

    };
    var map = new google.maps.Map(document.getElementById(div_map), mapProp);

    return map;
}



function initializeGoogleMap_WithZoom(div_map, myCenter, Zoom) {

    // var myCenter = new google.maps.LatLng(30.056476,31.378389999999968);
    //create the map
    var map = CreateMap_WithZoom(myCenter, div_map, Zoom);
    return map;
}

//Create Map ******************************//
function CreateMap_WithZoom(myCenter, div_map, Zoom) {
    var mapProp = {
        center: myCenter,
        zoom: Zoom,
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        disableDefaultUI: true,

        panControl: true,
        panControlOptions: {
            position: google.maps.ControlPosition.TOP_RIGHT
        },
        zoomControl: true,
        zoomControlOptions: {
            style: google.maps.ZoomControlStyle.DEFAULT,
            position: google.maps.ControlPosition.LEFT_CENTER
        },
        mapTypeControl: true,
        mapTypeControlOptions: {
            style: google.maps.MapTypeControlStyle.HORIZONTAL_BAR,
            position: google.maps.ControlPosition.TOP_RIGHT,
            mapTypeIds: [google.maps.MapTypeId.ROADMAP, google.maps.MapTypeId.HYBRID, google.maps.MapTypeId.SATELLITE]
        },
        scaleControl: true,
        scaleControlOptions: {
            position: google.maps.ControlPosition.TOP_LEFT
        },
        streetViewControl: true,
        streetViewControlOptions: {
            position: google.maps.ControlPosition.LEFT_TOP
        },
        overviewMapControl: true

    };
    var map = new google.maps.Map(document.getElementById(div_map), mapProp);

    return map;
}
//**********************************************************************//
//                             Begin  Markers
//**********************************************************************//
function ClearAllMarkers() {
    for (var i = 0; i < AllMarkers.length; i++) {
        AllMarkers[i].setMap(null);
    }
}

//Create Marker  ********************************//
function CreateIconMarker(map, Point, icon) {

    //With Icon********************************
    var marker = new google.maps.Marker({
        position: Point,
        title: "Click me",
        icon: icon,
        draggable: true,
        animation: google.maps.Animation.DROP
    });
    marker.setMap(map);

    return marker;
}

function CreateDefaultMarker(map, Point) {


    //with Default icon********************************
    var marker = new google.maps.Marker({
        title: "Click me",
        position: Point
    });
    marker.setMap(map);

    return marker;
}


//**********************************************************************//
//                             End  Markers
//**********************************************************************//




//**********************************************************************//
//                             Begin  InfoWindow
//**********************************************************************//
//Create InfoWindo*********************************//
function CreateInfoWindow(map, marker, content, IsOpen) {
    var content = content;
    var infowindow = new google.maps.InfoWindow({
        content: content
    });

    if (IsOpen == true)
        infowindow.open(map, marker);

    return infowindow;
}
//**********************************************************************//
//                             End  InfoWindow
//**********************************************************************//





//Create PolyLine **************************************// خط السير
//var Point_1 = new google.maps.LatLng(58.983991, 5.734863);
//var Point_2= new google.maps.LatLng(52.395715, 4.888916);
//var Point_3 = new google.maps.LatLng(51.508742, -0.120850);

//var myTrip = [Point_1, Point_2, Point_3];
//var StrockColor = "#0000FF";
function CreatePolyLine(map, myTrip, strokeColor) {
    var lineSymbol = {
        path: google.maps.SymbolPath.FORWARD_CLOSED_ARROW
    };
    var flightPath = new google.maps.Polyline({
        path: myTrip,
        icons: [
                 { icon: lineSymbol, offset: '5%' },
                 { icon: lineSymbol, offset: '10%' },
                 { icon: lineSymbol, offset: '15%' },
                 { icon: lineSymbol, offset: '20%' },
                 { icon: lineSymbol, offset: '25%' },
                 { icon: lineSymbol, offset: '30%' },
                 { icon: lineSymbol, offset: '35%' },
                 { icon: lineSymbol, offset: '40%' },
                 { icon: lineSymbol, offset: '45%' },
                 { icon: lineSymbol, offset: '50%' },
                 { icon: lineSymbol, offset: '55%' },
                 { icon: lineSymbol, offset: '60%' },
                 { icon: lineSymbol, offset: '65%' },
                 { icon: lineSymbol, offset: '70%' },
                 { icon: lineSymbol, offset: '75%' },
                 { icon: lineSymbol, offset: '80%' },
                 { icon: lineSymbol, offset: '85%' },
                 { icon: lineSymbol, offset: '90%' },
                 { icon: lineSymbol, offset: '95%' },

        ],
        geodesic: true,
        strokeColor: strokeColor,
        strokeOpacity: 0.8,
        strokeWeight: 2
    });

    flightPath.setMap(map);

    return flightPath;
}

//    Sample     //
//*****************************
//  save data in marker
//Marker.set('id','5');
//var id=Marker.get('id');
//***************


function RegisterFullScreenModeAndExitFullScreenMode(map, div_mapID, latlng) {
    var googleMapWidth = $('#' + div_mapID).css('width');
    var googleMapHeight = $('#' + div_mapID).css('height');

    var FullScreenControlDiv = document.createElement('div');
    var NormalScreenControlDiv = document.createElement('div');
    $(NormalScreenControlDiv).hide();

    var fullScreenControl = new FullScreenControl(FullScreenControlDiv, map, NormalScreenControlDiv, div_mapID, latlng);
    var normalScreenControl = new NormalScreenControl(NormalScreenControlDiv, map, FullScreenControlDiv, div_mapID, latlng, googleMapWidth, googleMapHeight);

    FullScreenControlDiv.index = 1;
    map.controls[google.maps.ControlPosition.BOTTOM_LEFT].push(FullScreenControlDiv);

    NormalScreenControlDiv.index = 1;
    map.controls[google.maps.ControlPosition.BOTTOM_LEFT].push(NormalScreenControlDiv);



}


function FullScreenControl(controlDiv, map, NormalScreenControlDiv, div_mapID, latlng) {

    // Set CSS styles for the DIV containing the control
    // Setting padding to 5 px will offset the control
    // from the edge of the map
    controlDiv.style.padding = '5px';
    // controlDiv.style.top = '355px';

    // Set CSS for the control border
    var controlUI = document.createElement('div');
    controlUI.style.backgroundColor = 'white';
    //  controlUI.style.borderStyle = 'solid';
    // controlUI.style.borderWidth = '1px';
    controlUI.style.cursor = 'pointer';
    controlUI.style.textAlign = 'center';
    controlUI.title = 'Click to Show Full Screen Mode';
    controlDiv.appendChild(controlUI);

    // Set CSS for the control interior
    var controlImg = document.createElement('img');
    controlImg.style.width = '20px';
    controlImg.src = '../../Content/images/FullScreen.png';
    controlUI.appendChild(controlImg);

    // Setup the click event listeners: simply set the map to
    // Chicago
    google.maps.event.addDomListener(controlUI, 'click', function () {
        // map.setCenter(chicago)
        ShowFullScreen(div_mapID, latlng, map);
        $(NormalScreenControlDiv).show();
        $(controlDiv).hide();
    });

}
function NormalScreenControl(controlDiv, map, FullScreenControlDiv, div_mapID, latlng, googleMapWidth, googleMapHeight) {

    // Set CSS styles for the DIV containing the control
    // Setting padding to 5 px will offset the control
    // from the edge of the map
    controlDiv.style.padding = '5px';
    //controlDiv.style.top = '355px';

    // Set CSS for the control border
    var controlUI = document.createElement('div');
    controlUI.style.backgroundColor = 'white';
    //  controlUI.style.borderStyle = 'solid';
    // controlUI.style.borderWidth = '1px';
    controlUI.style.cursor = 'pointer';
    controlUI.style.textAlign = 'center';
    controlUI.title = 'Click to Exite Full Screen Mode';
    controlDiv.appendChild(controlUI);

    // Set CSS for the control interior
    var controlImg = document.createElement('img');
    controlImg.style.width = '20px';
    controlImg.src = '../../Content/images/NormalScreen.png';
    controlUI.appendChild(controlImg);

    // Setup the click event listeners: simply set the map to
    // Chicago
    google.maps.event.addDomListener(controlUI, 'click', function () {
        // map.setCenter(chicago)
        ShowNormalScreen(div_mapID, latlng, map, googleMapWidth, googleMapHeight);
        $(FullScreenControlDiv).show();
        $(controlDiv).hide();
    });

}
function ShowFullScreen(div_mapID, latlng, map) {

    $("#" + div_mapID).css("position", 'fixed').
       css('top', 0).
       css('left', 0).
       css("width", '100%').
       css("height", '100%').css("z-index", "100");
    google.maps.event.trigger(map, 'resize');
    map.setCenter(latlng);
}
function ShowNormalScreen(div_mapID, latlng, map, googleMapWidth, googleMapHeight) {
    $("#" + div_mapID).css("position", 'relative').
           css('top', 0).
           css("width", googleMapWidth).
           css("height", googleMapHeight).css("z-index", "");
    google.maps.event.trigger(map, 'resize');
    map.setCenter(latlng);
}


//Create Click Event on marker *********************************//
function CreateClickEventOnMarker(marker, map, InfoWindow) {
    google.maps.event.addListener(marker, 'click', function () {
        map.setZoom(20);
        map.setCenter(marker.getPosition());
        InfoWindow.open(map, marker);
    });
}


//*************gET DURATION and distance
function calculateAndDisplayRoute_Distance_Duration(ShowOnMap_Map, pointA, pointB, div_Time_Distance, ExtimatedTimeText, KiloText,HourText,MinuteText) {

    var directionsService = new google.maps.DirectionsService;
    var directionsDisplay = new google.maps.DirectionsRenderer({
        map: ShowOnMap_Map
    });

    directionsService.route({
        origin: pointA,
        destination: pointB,
        travelMode: google.maps.TravelMode.DRIVING
    }, function (response, status) {
        if (status == google.maps.DirectionsStatus.OK) {
            directionsDisplay.setDirections(response);
            
            var point = response.routes[0].legs[0];
            var Duration = point.duration.text;
            var Distance = parseFloat(point.distance.text);

            Duration = Duration.replace("hours", HourText);
            Duration = Duration.replace("hour", HourText);
            Duration = Duration.replace("mins", MinuteText);

            $('#' + div_Time_Distance).html(ExtimatedTimeText + ': ' + Duration + ' -' + Distance + ' ' + KiloText );
        } else {
            window.alert('Directions request failed due to ' + status);
        }
    });
}