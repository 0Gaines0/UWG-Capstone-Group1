document.addEventListener("DOMContentLoaded", function () {
    function loadSortable(callback) {
        let script = document.createElement("script");
        script.src = "https://cdnjs.cloudflare.com/ajax/libs/Sortable/1.15.2/Sortable.min.js";
        script.onload = callback;
        document.head.appendChild(script);
    }

    loadSortable(function () {
        let board = document.getElementById("board");

        if (board) {
            new Sortable(board, {
                animation: 150,
                handle: ".column-header",
                onEnd: function () {
                    let stateOrder = [];
                    document.querySelectorAll(".column").forEach((column, index) => {
                        stateOrder.push({
                            stateId: column.getAttribute("data-stateid"),
                            position: index
                        });
                    });

                    fetch("/Projects/UpdateBoardStateOrder", {
                        method: "POST",
                        headers: { "Content-Type": "application/json" },
                        body: JSON.stringify(stateOrder),
                    }).catch((error) => console.error("Error updating order:", error));
                },
            });
        }
    });

    function enableStateRename(element) {
        element.addEventListener("dblclick", function () {
            let stateId = this.closest(".column").dataset.stateid;
            let currentText = this.innerText.trim();

            let input = document.createElement("input");
            input.type = "text";
            input.value = currentText;
            input.classList.add("edit-state");
            input.style.width = "90%";
            input.style.fontSize = "inherit";

            this.replaceWith(input);
            input.focus();

            function saveStateName() {
                let newText = input.value.trim() || "Unnamed State";
                let newH3 = document.createElement("h3");
                newH3.innerText = newText;
                newH3.dataset.stateid = stateId;
                newH3.classList.add("state-name");

                input.replaceWith(newH3);

                enableStateRename(newH3);

                fetch("/Projects/UpdateStateName", {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({ id: stateId, name: newText }),
                }).catch((error) => console.error("Error updating state name:", error));
            }

            input.addEventListener("keypress", function (e) {
                if (e.key === "Enter") saveStateName();
            });

            input.addEventListener("blur", saveStateName);
        });
    }

    function attachEventListeners() {
        document.querySelectorAll(".column-header h3").forEach(enableStateRename);
    }

    attachEventListeners();

    window.removeState = function (stateId) {
        if (confirm("Are you sure you want to delete this state?")) {
            fetch("/Projects/DeleteState", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ id: stateId }),
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        console.log("State deleted successfully:", data.message);
                        let stateElement = document.getElementById(`state-${stateId}`);
                        if (stateElement) {
                            stateElement.remove();
                        }

                        reloadBoard();
                    } else {
                        console.error("Failed to delete state:", data.message);
                    }
                })
                .catch(error => console.error("Error deleting state:", error));
        }
    };

    function reloadBoard() {
        fetch("/Projects/GetBoardStates")
            .then(response => response.json())
            .then(states => {
                const board = document.getElementById("board");
                board.innerHTML = "";

                states.forEach(state => {
                    createNewColumn(state.stateId, state.stateName);
                });

                console.log("Board refreshed with updated states.");
            })
            .catch(error => console.error("Error fetching updated board:", error));
    }


    const board = document.getElementById("board");
    const addStateButton = document.getElementById("add-state-btn");

    if (!board) {
        console.error("Board element not found! Ensure <div id='board'> exists.");
        return;
    }

    const boardId = board.getAttribute("data-boardid");
    if (!boardId) {
        console.error("Board ID not found! Ensure <div id='board' data-boardid='...'> exists.");
        return;
    }

    if (addStateButton) {
        addStateButton.addEventListener("click", function () {
            let stateName = prompt("Enter a name for the new column:");
            if (!stateName || stateName.trim() === "") {
                alert("Column name cannot be empty.");
                return;
            }

            fetch("/Projects/AddState", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({ name: stateName, boardId: boardId }),
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        createNewColumn(data.stateId, stateName);
                    } else {
                        console.error("Error creating state:", data.message);
                    }
                })
                .catch(error => console.error("Error creating state:", error));
        });
    } else {
        console.error("Add State button not found! Check the button ID in the HTML.");
    }

    function createNewColumn(stateId, stateName) {
        let column = document.createElement("div");
        column.classList.add("column");
        column.id = `state-${stateId}`;
        column.dataset.stateid = stateId;

        let columnHeader = document.createElement("div");
        columnHeader.classList.add("column-header");

        let h3 = document.createElement("h3");
        h3.innerText = stateName;
        h3.classList.add("state-name");
        enableStateRename(h3);

        let deleteButton = document.createElement("button");
        deleteButton.classList.add("delete-btn");
        deleteButton.innerHTML = "✖";
        deleteButton.onclick = function () {
            removeState(stateId);
        };

        columnHeader.appendChild(h3);
        columnHeader.appendChild(deleteButton);
        column.appendChild(columnHeader);

        board.appendChild(column);
    }





});

window.assignGroupToState = function () {
    const stateId = document.getElementById("state-select").value;
    const groupId = document.getElementById("group-select").value;

    fetch(`/Groups/AssignGroups`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            stateId: parseInt(stateId),
            groupIds: [parseInt(groupId)]
        })
    })
        .then(response => response.json())
        .then(data => {
            alert(data.message);
            location.reload();
        })
        .catch(error => {
            console.error('Error assigning group:', error);
            alert('Failed to assign group.');
        });
}

window.removeGroupFromState = function (stateId, groupId) {
    fetch(`/Groups/RemoveStateGroup`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            stateId: parseInt(stateId),
            groupIds: [parseInt(groupId)]
        })
    })
        .then(response => {
            if (!response.ok) {
                return response.json().then(err => { throw new Error(err.message); });
            }
            return response.json();
        })
        .then(data => {
            alert(data.message);
            location.reload();
        })
        .catch(error => {
            console.error('Error removing group:', error);
            alert('Failed to remove group: ' + error.message);
        });
}

