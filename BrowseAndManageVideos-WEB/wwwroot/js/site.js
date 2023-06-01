function init() {
    document.getElementById("btn-search").onclick = async function () { search() };
    //document.getElementById("btn-open-file").onclick = function () { openFileInDirectory() };
    document.getElementById("checkbox-all").onclick = function () { checkAll() };
    resizableGrid(document.getElementById("movie-table"));
}

async function search() {
    var param = document.getElementById("text-search").value;
    if (param == "" || param == null) {
        return;
    }
    fetch(`https://localhost:44378/Movie/Search/${param}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
    })
        .then(response => {
            if (response.ok) {
                return response.text(); // Extract the response data as text
            } else {
                throw new Error('An error occurred while fetching the movie data.');
            }
        })
        .then(data => {
            // Update the container element with the fetched partial view
            document.getElementById('movie-table').innerHTML = data;
            console.log('Movie data successfully updated on the page.');
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

async function toggleUpdateModal(id) {
    // Get modal
    var modal = document.getElementById("update-modal");

    // Get input
    var inputID = document.getElementById("update-id");
    var inputName = document.getElementById("update-name");
    var inputDescription = document.getElementById("update-description");
    var inputActor = document.getElementById("update-actor");
    var inputRating = document.getElementById("update-rating");

    await fetch(`https://localhost:44378/Movie/${id}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
    })
    .then(async response => {
        if (response.ok) {
            const data = await response.json();
            // Handle the retrieved data
            inputID.value = data.id;
            inputName.value = data.name;
            inputDescription.value = data.description;
            inputActor.value = data.actor;
            inputRating.value = data.rating;
        } else {
            throw new Error('An error occurred while fetching the movie data.');
        }
    })
    .catch(error => {
        console.error('An error occurred:', error);
    });

    // Set default input

    // Get the <span> element that closes the modal
    var span = document.getElementsByClassName("close")[0];
    var btnCancel = document.getElementById("btn-cancel");
    var btnUpdate = document.getElementById("btn-update");
    modal.style.display = "block";
    // When the user clicks the button, open the modal 

    // When the user clicks on <span> (x), close the modal
    span.onclick = function () {
        modal.style.display = "none";
    }

    // When the user clicks anywhere outside of the modal, close it
    window.onclick = function (event) {
        if (event.target == modal) {
            modal.style.display = "none";
        }
    }
    btnCancel.onclick = function () {
        modal.style.display = "none";
    }

    btnUpdate.onclick = async function () {
        if (inputName == null || inputName == "") {
            alert("File name can not be null!");
            return;
        } else {
            fetch(`https://localhost:44378/Movie/Update`, {
                method: 'PATCH',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    "Id": inputID.value,
                    "Name": inputName.value,
                    "Description": inputDescription.value,
                    "Actor": inputActor.value,
                    "Rating": inputRating.value
                })
            })
                .then(response => response.json())
                .then(async data => {
                    // Handle the response data here
                    console.log(data);
                    await search();
                })
                .catch(error => {
                    // Handle any errors that occur during the request
                    console.error('Error:', error);
                });
        }
        modal.style.display = "none";
    }

}

async function processOption(event) {
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
            toggleUpdateModal(movieID);
            
            document.getElementById(event.target.id).selectedIndex = "0";
            break;
        case "delete":
            var movieID = event.target.id.replace('options-', '');
            fetch(`https://localhost:44378/Movie/Delete?id=${movieID}`, {
                method: 'DELETE',
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
            fetch(`https://localhost:44378/Movie/Insertall`, {
                method: 'GET',
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
    const paginationNumbers = document.getElementById("pagination-numbers");
    const paginatedList = document.getElementById("paginated-list");
    const listItems = paginatedList.querySelectorAll("li");
    const nextButton = document.getElementById("next-button");
    const prevButton = document.getElementById("prev-button");

    const paginationLimit = 10;
    const pageCount = Math.ceil(listItems.length / paginationLimit);
    let currentPage = 1;

    const disableButton = (button) => {
        button.classList.add("disabled");
        button.setAttribute("disabled", true);
    };

    const enableButton = (button) => {
        button.classList.remove("disabled");
        button.removeAttribute("disabled");
    };

    const handlePageButtonsStatus = () => {
        if (currentPage === 1) {
            disableButton(prevButton);
        } else {
            enableButton(prevButton);
        }

        if (pageCount === currentPage) {
            disableButton(nextButton);
        } else {
            enableButton(nextButton);
        }
    };

    const handleActivePageNumber = () => {
        document.querySelectorAll(".pagination-number").forEach((button) => {
            button.classList.remove("active");
            const pageIndex = Number(button.getAttribute("page-index"));
            if (pageIndex == currentPage) {
                button.classList.add("active");
            }
        });
    };

    const appendPageNumber = (index) => {
        const pageNumber = document.createElement("button");
        pageNumber.className = "pagination-number";
        pageNumber.innerHTML = index;
        pageNumber.setAttribute("page-index", index);
        pageNumber.setAttribute("aria-label", "Page " + index);

        paginationNumbers.appendChild(pageNumber);
    };

    const getPaginationNumbers = () => {
        for (let i = 1; i <= pageCount; i++) {
            appendPageNumber(i);
        }
    };

    const setCurrentPage = (pageNum) => {
        currentPage = pageNum;

        handleActivePageNumber();
        handlePageButtonsStatus();

        const prevRange = (pageNum - 1) * paginationLimit;
        const currRange = pageNum * paginationLimit;

        listItems.forEach((item, index) => {
            item.classList.add("hidden");
            if (index >= prevRange && index < currRange) {
                item.classList.remove("hidden");
            }
        });
    };

    window.addEventListener("load", () => {
        getPaginationNumbers();
        setCurrentPage(1);

        prevButton.addEventListener("click", () => {
            setCurrentPage(currentPage - 1);
        });

        nextButton.addEventListener("click", () => {
            setCurrentPage(currentPage + 1);
        });

        document.querySelectorAll(".pagination-number").forEach((button) => {
            const pageIndex = Number(button.getAttribute("page-index"));

            if (pageIndex) {
                button.addEventListener("click", () => {
                    setCurrentPage(pageIndex);
                });
            }
        });
    });
}

window.onload = function () {
    init();
}



