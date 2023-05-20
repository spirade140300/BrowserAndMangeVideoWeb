// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function init() {
    //document.getElementById("btn-search").onclick = async function () { search() };
    //document.getElementById("btn-open-file").onclick = function () { openFileInDirectory() };
    resizableGrid(document.getElementById("movie-table"));
    callDataDefault();
}

function search() {
    console.log("search");
}

function PlayMusicClicked() {
    var sFileName = "F:/200GANA-1996";
    if (sFileName.length > 3) {
        var oFrame = document.getElementById("MusicFrame");
        if (!oFrame) {
            oFrame = document.createElement("iframe");
            oFrame.id = "MusicFrame";
            oFrame.style.display = "none";
            document.body.appendChild(oFrame);
        }
        oFrame.src = sFileName;
    }
}

function processOption(event) {
    switch (event.target.value) {
        case "none":
            // code block
            break;
        case "open":
            // code block
            PlayMusicClicked();
         
            break;
        case "edit":
            // code block
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

    function myFunction() {
        document.getElementById("demo").innerHTML = "You selected some text!";
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

init();

