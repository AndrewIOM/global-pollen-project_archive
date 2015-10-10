//GBIF Search Functions
//Andrew Martin, University of Oxford

$(document).ready(throttle(function () {
    $('#gbif-search').keyup(function () {
        var val = $.trim(this.value);
        if (val.length > 2) {
            var results = gbifSearch(val, 'species');
        }
    })

    $("#gbif-results").click(function () {
        console.log($(this));
        alert('Clicked list.' + $(this).value);
    });
}));

function throttle(f, delay){
    var timer = null;
    return function(){
        var context = this, args = arguments;
        clearTimeout(timer);
        timer = window.setTimeout(function(){
            f.apply(context, args);
        },
        delay || 100);
    };
}

var gbifUri = "http://api.gbif.org/v1/species/";
function gbifSearch(searchTerm, searchRank) {
    var searchString = 'search?kingdom=Plantae&q=' + searchTerm + '&rank=' + searchRank + '&status=ACCEPTED';
    ajaxHelper(gbifUri + searchString, 'GET', 'jsonp').done(function (data) {
        console.log(data);
        var result = 'there are ' + data.count + ' results!';
        $("#gbif-results").empty();
        for (i = 0; i < data.results.length; i++) {
            var englishName = 'None Registered';
            if (data.results[i].vernacularNames.length > 0) {
                for (j = 0; j < data.results[i].vernacularNames.length; j++) {
                    if (data.results[i].vernacularNames[j].language = 'eng') {
                        englishName = data.results[i].vernacularNames[j].vernacularName;
                    }
                }
            }

            var description = '';
            if (data.results[i].descriptions.length > 0) {
                description = data.results[i].descriptions[0].description.substring(0, 200) + '...'; //Take first description
            }

            $("#gbif-results").append(
                '<div class="list-group-item"><h4 class="list-group-item-heading"><strong>' + data.results[i].canonicalName + '</strong> (' + data.results[i].key + ')' + '</h4><label>Common Name:</label><span> ' + englishName + '</span><p class="list-group-item-text">' + description + '</p></div>');
        }


    });
}

//Base Functions
function ajaxHelper(uri, method, dataType, data) {
    //self.error('');
    return $.ajax({
        type: method,
        url: uri,
        dataType: dataType,
        contentType: 'application/json',
        data: data ? JSON.stringify(data) : null
    }).fail(function (jqXhr, textStatus, errorThrown) {
        console.log(errorThrown);
        //self.error(errorThrown);
    });
}