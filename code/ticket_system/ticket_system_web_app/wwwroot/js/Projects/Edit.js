document.addEventListener("DOMContentLoaded", async function () {
    alert("UWU")
    await fetchAllGroupsInitial();
});

let selectedGroups = []
let unselectedGroups = []
let selectedManagerCount = {}
let managerNames = {}

async function fetchAllGroupsInitial() {
    let initialCollabIDs = []
    let collabTableRows = document.querySelectorAll("#collaboratorsTableBody tr");

    collabTableRows.forEach(currRow => {
        let collabId = parseInt(currRow.id.split("-").pop());
        initialCollabIDs.push(collabId);
    });

    await fetch("/Groups/GetAllGroups").then(response => response.json()).then(data => {
        data.forEach(group => {
            if (selectedManagerCount[group.managerId] == null || selectedManagerCount[group.managerId] == NaN) {
                selectedManagerCount[group.managerId] = 0;
            }
            if (initialCollabIDs.includes(group.gId)) {
                selectedGroups.push(group);
                selectedManagerCount[group.managerId]++;
            } else {
                unselectedGroups.push(group);
            }
            managerNames[group.managerId] = group.managerName;
        });
    }).catch(_error => alert("Error fetching groups."));

    populateTables();
}



function populateTables() {
    sortGroupCollection(unselectedGroups);
    sortGroupCollection(selectedGroups);
    populateUnselectedTable();
    populateSelectedTable();
    populateManagersList();
}

function sortGroupCollection(groups) {
    groups.sort((a, b) => a.gName.localeCompare(b.gName));
}

function populateUnselectedTable() {
    let unselectedGroupsTableBody = document.getElementById("groupsTableBody");
    unselectedGroupsTableBody.innerHTML = "";

    unselectedGroups.forEach(group => {
        let row = `
                <tr>
                    <td>${group.gName} ${group.gId}</td>
                    <td><button class="btn" type="button" onClick="addCollaborator(${group.gId});">Add</button></td>
                </tr>`;
        unselectedGroupsTableBody.innerHTML += row;
    });
}

function populateSelectedTable() {
    let selectedGroupsTableBody = document.getElementById("collaboratorsTableBody");
    selectedGroupsTableBody.innerHTML = "";

    selectedGroups.forEach(group => {
        let row = `
                <tr>
                    <td><button class="btn" type="button" onClick="removeCollaborator(${group.gId});">Remove</button></td>
                    <td>${group.gName}</td>
                </tr>`;
        selectedGroupsTableBody.innerHTML += row;
    });
}

function populateManagersList() {
    let projectLeadSelect = document.getElementById("projectLeadSelect");
    projectLeadSelect.innerHTML = `<option selected disabled value="0">Select a project lead</option>`;

    for (let mid in selectedManagerCount) {
        if (selectedManagerCount[mid] > 0) {
            let managersRow = `<option value="${mid}">${managerNames[mid]}</option>`;
            projectLeadSelect.innerHTML += managersRow;
        }
    }
}



function addCollaborator(id) {
    collaboratorIndex = unselectedGroups.findIndex(group => group.gId == id);
    selectedGroups.push(unselectedGroups[collaboratorIndex]);
    unselectedGroups.splice(collaboratorIndex, 1);

    selectedManagerCount[selectedGroups[selectedGroups.length - 1].managerId]++;

    populateTables();
}

function removeCollaborator(id) {
    collaboratorIndex = selectedGroups.findIndex(group => group.gId == id);
    unselectedGroups.push(selectedGroups[collaboratorIndex]);
    selectedGroups.splice(collaboratorIndex, 1);

    selectedManagerCount[unselectedGroups[unselectedGroups.length - 1].managerId]--;

    populateTables();
}



function handleSave() {
    if (validateChanges()) {
        let collaboratorsCache = document.getElementById("collaboratorsCache");
        selectedGroups.forEach(group => {
            collaboratorsCache.value += group.gId + ",";
        });
        collaboratorsCache.value = collaboratorsCache.value.slice(0, collaboratorsCache.value.length - 1);
        alert("Project edited successfully!");
        document.getElementById("editForm").submit();
    }
}

function validateChanges() {
    let invalidValues = false;
    if (!document.getElementById("PTitleInput").value.trim()) {
        invalidValues = true;
        document.getElementById("PTitleErr").innerText = "Please enter a title.";
    } else {
        document.getElementById("PTitleErr").innerText = "";
    }
    if (!document.getElementById("PDescriptionInput").value.trim()) {
        invalidValues = true;
        document.getElementById("PDescriptionErr").innerText = "Please enter a description.";
    } else {
        document.getElementById("PDescriptionErr").innerText = "";
    }
    if (document.getElementById("projectLeadSelect").value < 1) {
        invalidValues = true;
        document.getElementById("ProjectLeadErr").innerText = "Please select a project lead.";
    } else {
        document.getElementById("ProjectLeadErr").innerText = "";
    }
    return !invalidValues;
}