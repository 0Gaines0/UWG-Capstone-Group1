document.addEventListener("DOMContentLoaded", async function () {
    fetchAllGroups();
});

let selectedGroups = []
let unselectedGroups = []
let selectedManagerCount = {}
let managerNames = {}

async function fetchAllGroups() {
    await fetch("/Groups/GetAllGroups").then(response => response.json()).then(data => {
        data.forEach(group => {
            unselectedGroups.push(group);
            selectedManagerCount[group.managerId] = 0;
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



async function createProject() {
    let collaboratorIDs = []
    selectedGroups.forEach(group => {
        collaboratorIDs.push(group.gId);
    });

    let pLeadID = document.getElementById("projectLeadSelect").value;
    const projectData = {
        PTitle: document.getElementById("projectTitle").value.trim(),
        PDescription: document.getElementById("projectDescription").value.trim(),
        PLeadID: pLeadID ? pLeadID : 0,
        CollaboratingGroupIDs: collaboratorIDs
    };

    let invalidValues = false;
    if (!projectData.PTitle.trim()) {
        invalidValues = true;
        document.getElementById("projectTitleErr").innerText  = "Please enter a title.";
    }
    if (!projectData.PDescription.trim()) {
        invalidValues = true;
        document.getElementById("projectDescriptionErr").innerText  = "Please enter a description.";
    }
    if (projectData.PLeadID < 1) {
        invalidValues = true;
        document.getElementById("projectLeadErr").innerText = "Please select a project lead.";
    }
    if (invalidValues) {
        return;
    }

    try {
        const response = await fetch("/Projects/CreateProject", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(projectData)
        });
        const result = await response.json();

        if (response.ok) {
            alert("Project created successfully!");
            document.getElementById("onCreateCompleteBtn").click();
        } else {
            alert(`Error: ${result.message}`);
        }
    } catch (error) {
        alert("An error occurred while creating the group.");
    }
}