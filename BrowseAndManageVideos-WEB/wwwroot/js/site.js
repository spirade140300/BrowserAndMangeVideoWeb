function init() {
    //document.getElementById("btn-search").onclick = async function () { search() };
    //document.getElementById("btn-open-file").onclick = function () { openFileInDirectory() };
    document.getElementById("checkbox-all").onclick = function () { checkAll() };
    resizableGrid(document.getElementById("movie-table"));
    processPagingUI();

}

function processPaging(event) {
    var page = document.querySelectorAll('[data]');
    var search = document.getElementById("text-search").value;
    var currentPage = event.getAttribute("data");
    switch (page) {
        case "first":
            (search == "") ? getMovieNoSearch(1) : getMovieWithSearch(1, search);

            break;
        case "backward":
            if (currentPage == 1) {
                return;
            }
            (search == "") ? getMovieNoSearch(currentPage) : getMovieWithSearch(currentPage, search);
            break;
        case "forward":
            (search == "") ? getMovieNoSearch(currentPage) : getMovieWithSearch(currentPage, search);
            break;
        case "last":
            if (currentPage == numberOfPages) {
                return;
            }
            (search == "") ? getMovieNoSearch(numberOfPages) : getMovieWithSearch(numberOfPages, search);
            break;
        default:
            (search == "") ? getMovieNoSearch(currentPage) : getMovieWithSearch(currentPage, search);
    }
}

function processPagingUI() {
    var numberOfPages = document.getElementsByName("paging").length - 4;
    var currentPage = document.getElementsByClassName("active")[0].getAttribute("data");
    var maxPage = 3;
    if (currentPage == 1) {
        var first = document.querySelector('[data="first"]').setAttribute("style", "background-color: #BCBCBC");
        var backward = document.querySelector('[data="backward"]').setAttribute("style", "background-color: #BCBCBC");
        if (numberOfPages > currentPage + maxPage) {
            for (let i = maxPage + 2; i < numberOfPages + 1; i++) {
                console.log(document.querySelector('[data="' + i + '"]').setAttribute("style", "display: none"));
            }
        }
    }
    for (let i = 2; i < numberOfPages; i++) {

    }

    if (currentPage == numberOfPages) {
        var last = document.querySelector('[data="last"]').setAttribute("style", "background-color: #BCBCBC");
        var forward = document.querySelector('[data="forward"]').setAttribute("style", "background-color: #BCBCBC");
    }
    
}

function getMovieNoSearch(page) {
    fetch(`https://localhost:44378/Movie/${page}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
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
}

function getMovieWithSearch(page, param) {
    fetch(`https://localhost:44378/Movie/${page}/s=${param}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
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
            var movieID = event.target.id.replace('options-', '');
            fetch(`https://localhost:44378/Movie/Openfile?id=${movieID}`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                },
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
            document.getElementById(event.target.id).selectedIndex = "0";
            break;
        case "edit":
            // code block
            var movieID = event.target.id.replace('options-', '');
            var newName = prompt("Update name/ path for Movie ID = " + movieID);
            if (newName == null || newName == "") {
                alert("File name can not be null!");
            } else {
                fetch(`https://localhost:44378/Movie/Update`, {
                    method: 'PATCH',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        "Id": movieID,
                        "Name": newName
                    })
                })
                    .then(response => response.json())
                    .then(data => {
                        // Handle the response data here
                        console.log(data);
                    })
                    .catch(error => {
                        // Handle any errors that occur during the request
                        console.error('Error:', error);
                    });
            }
            document.getElementById(event.target.id).selectedIndex = "0";
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
            document.getElementById("option-setting").selectedIndex = "0";
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

function paging (){
    const nextButton = document.getElementById("nextnext-page");
    const prevButton = document.getElementById("prev-page");


}

window.onload = function () {
    init();
}
