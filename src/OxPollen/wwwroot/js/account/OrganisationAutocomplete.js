function updateOrganisationList(entryBox) {
    ajax = new XMLHttpRequest();
    ajax.onreadystatechange = function () {
        if (ajax.readyState == 4 || ajax.readyState == "complete") {
            if (ajax.status == 200) {
                var result = ajax.responseText;
                var resultJson = JSON.parse(result);
                var orgList = document.getElementById('organisationsList');
                $('#organisationsList').css('display', '');
                orgList.innerHTML = "";
                if (!(resultJson.length == 1 && resultJson[0].Name == document.getElementById('Organisation').value)) {
                    for (var i = 0; i < resultJson.length; i++) {
                        var option = document.createElement('a');
                        option.innerHTML = resultJson[i].Name;
                        option.className = 'autocomplete-option';
                        option.addEventListener('click', function (e) {
                            var name = this.innerHTML;
                            console.log(name);
                            $('#Organisation').val(name);
                            $('#organisationsList').css('display', 'none');
                        });
                        orgList.appendChild(option);
                    }
                }
            }
            if (ajax.status == 400 || ajax.status == 500) {
            }
        }
    }

    var search = entryBox.value;
    ajax.open("GET", "/Organisation/Search?searchTerm=" + search);
    ajax.send();
}