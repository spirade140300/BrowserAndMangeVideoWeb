// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function init() {
    //document.getElementById("btn-search").onclick = async function () { search() };
    //document.getElementById("btn-open-file").onclick = function () { openFileInDirectory() };
    document.getElementById("checkbox-all").onclick = function () { checkAll() };
<<<<<<< HEAD
    
=======

>>>>>>> e8e9281
    resizableGrid(document.getElementById("movie-table"));
    callDataDefault();
}

function search() {
    console.log("search");
}

function checkAll() {
    checkboxes = document.getElementsByName("table-checkbox");
    for (var i = 0, n = checkboxes.length; i < n; i++) {
        if (checkboxes[i].checked) {
            checkboxes[i].checked = false;
        } else {
            checkboxes[i].checked = true;
        }
    }
}

function processOption(event) {
    switch (event.target.value) { //options-id
        case "none":
            // code block
            break;
        case "open":
            // code block
<<<<<<< HEAD
            fetch(`https://localhost:44378/movies/openfile=${event.target.id.replace('options-', '') }`, {
=======
            fetch(`https://localhost:44378/movies/openfile=${event.target.id.replace('options-', '')}`, {
>>>>>>> e8e9281
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(response => {
                    if (response.ok) {
                        console.log('File opened successfully.');
                    } else {
                        console.error('An error occurred while opening the file.');
                    }
                })
                .catch(error => {
                    console.error('An error occurred:', error);
                });
<<<<<<< HEAD
            
=======

>>>>>>> e8e9281
            break;
        case "edit":
            // code block
            var movieID = event.target.id.replace('options-', '');
            let newName = prompt("Update name/ path for Movie ID = " + movieID);
            if (newName == null || newName == "") {
                alert("User cancelled the prompt.");
            } else {
                alert("sucess");
            }
            break;
        case "select":
            // code block
            break;
        case "delete":
            // code block
            break;
        default:
        // code block
    }

}

function processOptionSetting(event) {
    switch (document.getElementById("option-setting").value) { //options-id
        case "none":
            // code block
            break;
        case "update":
            // code block
            fetch(`https://localhost:44378/movies/movies/insertall}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
<<<<<<< HEAD
            .then(response => {
                if (response.ok) {
                    console.log('File opened successfully.');
                } else {
                    console.error('An error occurred while opening the file.');
                }
            })
            .catch(error => {
                console.error('An error occurred:', error);
            });
=======
                .then(response => {
                    if (response.ok) {
                        console.log('File opened successfully.');
                    } else {
                        console.error('An error occurred while opening the file.');
                    }
                })
                .catch(error => {
                    console.error('An error occurred:', error);
                });
>>>>>>> e8e9281

            break;
        case "refresh":
            var text = document.getElementById("text-search").value();
            // code block
            fetch(`https://localhost:44378/movies/movies/insertall}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(response => {
                    if (response.ok) {
                        console.log('File opened successfully.');
                    } else {
                        console.error('An error occurred while opening the file.');
                    }
                })
                .catch(error => {
                    console.error('An error occurred:', error);
                });
            break;
        default:
        // code block
    }

}

function callDataDefault() {

}



function resizableGrid(table) {
    var row = table.getElementsByTagName('tr')[0],
        cols = row ? row.children : undefined;
    if (!cols) return;

    table.style.overflow = 'hidden';

    var tableHeight = table.offsetHeight;

    for (var i = 0; i < cols.length; i++) {
        var div = createDiv(tableHeight);
        cols[i].appendChild(div);
        cols[i].style.position = 'relative';
        setListeners(div);
    }

    function setListeners(div) {
        var pageX, curCol, nxtCol, curColWidth, nxtColWidth;

        div.addEventListener('mousedown', function (e) {
            curCol = e.target.parentElement;
            nxtCol = curCol.nextElementSibling;
            pageX = e.pageX;

            var padding = paddingDiff(curCol);

            curColWidth = curCol.offsetWidth - padding;
            if (nxtCol)
                nxtColWidth = nxtCol.offsetWidth - padding;
        });

        div.addEventListener('mouseover', function (e) {
            e.target.style.borderRight = '2px solid #0000ff';
        })

        div.addEventListener('mouseout', function (e) {
            e.target.style.borderRight = '';
        })

        document.addEventListener('mousemove', function (e) {
            if (curCol) {
                var diffX = e.pageX - pageX;

                if (nxtCol)
                    nxtCol.style.width = (nxtColWidth - (diffX)) + 'px';

                curCol.style.width = (curColWidth + diffX) + 'px';
            }
        });

        document.addEventListener('mouseup', function (e) {
            curCol = undefined;
            nxtCol = undefined;
            pageX = undefined;
            nxtColWidth = undefined;
            curColWidth = undefined
        });
    }

    function createDiv(height) {
        var div = document.createElement('div');
        div.style.top = 0;
        div.style.right = 0;
        div.style.width = '5px';
        div.style.position = 'absolute';
        div.style.cursor = 'col-resize';
        div.style.userSelect = 'none';
        div.style.height = height + 'px';
        return div;
    }

    function paddingDiff(col) {

        if (getStyleVal(col, 'box-sizing') == 'border-box') {
            return 0;
        }

        var padLeft = getStyleVal(col, 'padding-left');
        var padRight = getStyleVal(col, 'padding-right');
        return (parseInt(padLeft) + parseInt(padRight));

    }

    function getStyleVal(elm, css) {
        return (window.getComputedStyle(elm, null).getPropertyValue(css))
    }
};

window.onload = function () {
    init();
}
<<<<<<< HEAD

=======
>>>>>>> e8e9281

