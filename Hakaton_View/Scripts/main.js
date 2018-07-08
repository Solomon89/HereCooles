// ѕолучаем значение текущего положени€ человека (начало) 
var dataPoints;
goToMyDestination();
function goToMyDestination() {
    var latitude = 0;
    var longitude = 0;
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition);
    } else {
        x.innerHTML = "Geolocation is not supported by this browser.";
    }
}
var parisMarker;
function showPosition(position) {
    if (position == null) {
        latitude = 45.054899999999996;
        longitude = 41.993451199999996;
    }
    else
    {
        latitude = position.coords.latitude;
        longitude = position.coords.longitude;
    }

    executeFunction('/Map/GetWay', { x: latitude, y: longitude }, 'POST');

    if (parisMarker != null) {
        map.removeObject(parisMarker);
    }

    parisMarker = new H.map.Marker({ lat: latitude, lng: longitude });
    map.addObject(parisMarker);
    map.setCenter({ lat: latitude, lng: longitude });
    map.setZoom(17);
}
// ѕолучаем значение текущего положени€ человека (конец) 

// ¬ычислени€ пути (начало)
function calculateRouteFromAtoB(platform) {
    var router = platform.getRoutingService(),
        routeRequestParams = {
            mode: 'shortest;pedestrian',
            representation: 'display',
            routeattributes: 'waypoints,summary,shape,legs',
            maneuverattributes: 'direction,action',
            waypoint0: '45.0397,41.9702',
            waypoint1: '45.049572,41.978761',
            waypoint2: '45.049131,41.984018'
        };


    router.calculateRoute(
        routeRequestParams,
        onSuccess,
        onError
    );
}

function calculateRouteFromAtoBFromController(platform, data) {
    var router = platform.getRoutingService(),
        routeRequestParams = {
            mode: 'shortest;pedestrian',
            representation: 'display',
            routeattributes: 'waypoints,summary,shape,legs',
            maneuverattributes: 'direction,action'
        };
    var i = 0;
    dataPoints = data;
    data.forEach(function (item) {
        routeRequestParams["waypoint" + i] = item.X.toString() + "," + item.Y.toString();
        i = i + 1;
    });

    router.calculateRoute(
        routeRequestParams,
        onSuccess,
        onError
    );
}
// ¬ычислени€ пути (конец)
/**
 * This function will be called once the Routing REST API provides a response
 * @param  {Object} result          A JSONP object representing the calculated route
 *
 * see: http://developer.here.com/rest-apis/documentation/routing/topics/resource-type-calculate-route.html
 */
function onSuccess(result) {
    var route = result.response.route[0];
    /*
     * The styling of the route response on the map is entirely under the developer's control.
     * A representitive styling can be found the full JS + HTML code of this example
     * in the functions below:
     */
    addRouteShapeToMap(route);
    addManueversToMap(route);

    addWaypointsToPanel(route.waypoint);
    addManueversToPanel(route);
    addSummaryToPanel(route.summary);
    // ... etc.

}

/**
 * This function will be called if a communication error occurs during the JSON-P request
 * @param  {Object} error  The error message received.
 */
function onError(error) {
    alert('Ooops!');
}




/**
 * Boilerplate map initialization code starts below:
 */

// set up containers for the map  + panel
var mapContainer = document.getElementById('map'),
    routeInstructionsContainer = document.getElementById('panel');

//Step 1: initialize communication with the platform
var platform = new H.service.Platform({
    app_id: 'Uq4bfPVN8xSx4owJiAVK',
    app_code: 'fTtnmYAsLgDwaUysTP_uSA',
    useCIT: true,
    useHTTPS: true
});
var pixelRatio = window.devicePixelRatio || 1;
var defaultLayers = platform.createDefaultLayers({
    tileSize: pixelRatio === 1 ? 256 : 512,
    ppi: pixelRatio === 1 ? undefined : 320
});

//Step 2: initialize a map - this map is centered over Berlin
var map = new H.Map(mapContainer,
    defaultLayers.normal.map, {
        center: { lat: 45.043311, lng: 41.96903 },
        zoom: 13,
        pixelRatio: pixelRatio
    });

//Step 3: make the map interactive
// MapEvents enables the event system
// Behavior implements default interactions for pan/zoom (also on mobile touch environments)
var behavior = new H.mapevents.Behavior(new H.mapevents.MapEvents(map));

// Create the default UI components
var ui = H.ui.UI.createDefault(map, defaultLayers);

// Hold a reference to any infobubble opened
var bubble;

/**
 * Opens/Closes a infobubble
 * @param  {H.geo.Point} position     The location on the map.
 * @param  {String} text              The contents of the infobubble.
 */
function openBubble(position, text) {
    if (!bubble) {
        bubble = new H.ui.InfoBubble(
            position,
            // The FO property holds the province name.
            { content: text });
        ui.addBubble(bubble);
    } else {
        bubble.setPosition(position);
        bubble.setContent(text);
        bubble.open();
    }
}


/**
 * Creates a H.map.Polyline from the shape of the route and adds it to the map.
 * @param {Object} route A route as received from the H.service.RoutingService
 */
function addRouteShapeToMap(route) {
    var lineString = new H.geo.LineString(),
        routeShape = route.shape,
        polyline;

    routeShape.forEach(function (point) {
        var parts = point.split(',');
        lineString.pushLatLngAlt(parts[0], parts[1]);
    });

    polyline = new H.map.Polyline(lineString, {
        style: {
            lineWidth: 4,
            strokeColor: 'rgba(0, 128, 255, 0.7)'
        }
    });
    // Add the polyline to the map
    map.addObject(polyline);



    // And zoom to its bounding rectangle
    map.setViewBounds(polyline.getBounds(), true);

}


/**
 * Creates a series of H.map.Marker points from the route and adds them to the map.
 * @param {Object} route  A route as received from the H.service.RoutingService
 */
function addManueversToMap(route) {
    var svgMarkup1 = '<svg width="18" height="18" ' +
            'xmlns="http://www.w3.org/2000/svg">' +
            '<circle cx="8" cy="8" r="8" ' +
            'fill="#C840E9" stroke="white" stroke-width="1"  />' +
            '</svg>',
        dotIcon1 = new H.map.Icon(svgMarkup1, { anchor: { x: 15, y: 15 } }),
        group = new H.map.Group(),
        i,
        j;
    var svgMarkup2 = '<svg width="18" height="18" ' +
            'xmlns="http://www.w3.org/2000/svg">' +
            '<circle cx="8" cy="8" r="8" ' +
            'fill="#FF4F9A" stroke="white" stroke-width="1"  />' +
            '</svg>',
        dotIcon2 = new H.map.Icon(svgMarkup2, { anchor: { x: 15, y: 15 } });
    var svgMarkup3 = '<svg width="18" height="18" ' +
            'xmlns="http://www.w3.org/2000/svg">' +
            '<circle cx="8" cy="8" r="8" ' +
            'fill="#FF9057" stroke="white" stroke-width="1"  />' +
            '</svg>',
        dotIcon3 = new H.map.Icon(svgMarkup3, { anchor: { x: 15, y: 15 } });
    var svgMarkup4 = '<svg width="18" height="18" ' +
            'xmlns="http://www.w3.org/2000/svg">' +
            '<circle cx="8" cy="8" r="8" ' +
            'fill="#3ACCE1" stroke="white" stroke-width="1"  />' +
            '</svg>',
        dotIcon4 = new H.map.Icon(svgMarkup4, { anchor: { x: 15, y: 15 } });
    var svgMarkup5 = '<svg width="18" height="18" ' +
            'xmlns="http://www.w3.org/2000/svg">' +
            '<circle cx="8" cy="8" r="8" ' +
            'fill="#3497FD" stroke="white" stroke-width="1"  />' +
            '</svg>',
        dotIcon5 = new H.map.Icon(svgMarkup5, { anchor: { x: 15, y: 15 } });

    // Add a marker for each maneuver
    for (i = 0; i < route.leg.length; i += 1) {
        for (j = 0; j < route.leg[i].maneuver.length; j += 1) {
            // Get the next maneuver.
            maneuver = route.leg[i].maneuver[j];
            // Add a marker to the maneuvers group
            if (maneuver.action == "arrive") {
                var dotIcon;
                dataPoints.forEach(function(item) {
                    if (Math.abs(item.X - maneuver.position.latitude) <= 0.05
                        && Math.abs(item.Y - maneuver.position.longitude) <= 0.05) {
                        switch (item.PerformancePoint.Performance.Id) {
                            case 1:
                                dotIcon = dotIcon1;
                                break;
                            case 2:
                                dotIcon = dotIcon2;
                                break;
                            case 3:
                                dotIcon = dotIcon3;
                                break;
                            case 4:
                                dotIcon = dotIcon4;
                                break;
                            case 5:
                                dotIcon = dotIcon5;
                                break;
                            default:
                                break;
                        }
                    }
                });
                var marker = new H.map.Marker({
                    lat: maneuver.position.latitude,
                    lng: maneuver.position.longitude
                },
                    { icon: dotIcon });
                marker.instruction = maneuver.instruction;
                group.addObject(marker);
            }
            else {

            }


        }
    }

    group.addEventListener('tap', function (evt) {
        map.setCenter(evt.target.getPosition());
        openBubble(
            evt.target.getPosition(), evt.target.instruction);
    }, false);

    // Add the maneuvers group to the map
    map.addObject(group);
}


/**
 * Creates a series of H.map.Marker points from the route and adds them to the map.
 * @param {Object} route  A route as received from the H.service.RoutingService
 */
function addWaypointsToPanel(waypoints) {



    var nodeH3 = document.createElement('h3'),
        waypointLabels = [],
        i;


    for (i = 0; i < waypoints.length; i += 1) {
        waypointLabels.push(waypoints[i].label)
    }

    nodeH3.textContent = waypointLabels.join(' - ');

    routeInstructionsContainer.innerHTML = '';
    routeInstructionsContainer.appendChild(nodeH3);
}

/**
 * Creates a series of H.map.Marker points from the route and adds them to the map.
 * @param {Object} route  A route as received from the H.service.RoutingService
 */
function addSummaryToPanel(summary) {
    var summaryDiv = document.createElement('div'),
        content = '';
    content += '<b>Total distance</b>: ' + summary.distance + 'm. <br/>';
    content += '<b>Travel Time</b>: ' + summary.travelTime.toMMSS() + ' (in current traffic)';


    summaryDiv.style.fontSize = 'small';
    summaryDiv.style.marginLeft = '5%';
    summaryDiv.style.marginRight = '5%';
    summaryDiv.innerHTML = content;
    routeInstructionsContainer.appendChild(summaryDiv);
}

/**
 * Creates a series of H.map.Marker points from the route and adds them to the map.
 * @param {Object} route  A route as received from the H.service.RoutingService
 */
function addManueversToPanel(route) {



    var nodeOL = document.createElement('ol'),
        i,
        j;

    nodeOL.style.fontSize = 'small';
    nodeOL.style.marginLeft = '5%';
    nodeOL.style.marginRight = '5%';
    nodeOL.className = 'directions';

    // Add a marker for each maneuver
    for (i = 0; i < route.leg.length; i += 1) {
        for (j = 0; j < route.leg[i].maneuver.length; j += 1) {
            // Get the next maneuver.
            maneuver = route.leg[i].maneuver[j];

            var li = document.createElement('li'),
                spanArrow = document.createElement('span'),
                spanInstruction = document.createElement('span');

            spanArrow.className = 'arrow ' + maneuver.action;
            spanInstruction.innerHTML = maneuver.instruction;
            li.appendChild(spanArrow);
            li.appendChild(spanInstruction);

            nodeOL.appendChild(li);
        }
    }

    routeInstructionsContainer.appendChild(nodeOL);
}


Number.prototype.toMMSS = function () {
    return Math.floor(this / 60) + ' minutes ' + (this % 60) + ' seconds.';
}

// Now use the map as required...
//calculateRouteFromAtoB(platform);

function executeFunction(url, testData, method) {

    $.ajax({
        type: method,
        url: url,
        data: JSON.stringify(testData),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            calculateRouteFromAtoBFromController(platform, data);
        },
        error: function (errorData) {
            calculateRouteFromAtoBFromController(platform, errorData.responseText);
        }
    });
}