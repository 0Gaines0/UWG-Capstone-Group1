document.addEventListener("DOMContentLoaded", async function () {
    fetchAllGroups();
});

let selectedGroups = {}
let unselectedGroups = {}
let selectedManagerCount = {}
let managerNames = {}

async function fetchAllGroups() {
    await fetch("/Groups/GetAllGroups").then(response => response.json()).then(data => {
        data.forEach(group => {
            unselectedGroups[group.gId] = group;
            selectedManagerCount[group.managerId] = 0;
            managerNames[group.managerId] = group.managerName;
        });
    }).catch(_error => alert("Error fetching groups."));

    populateTables();
}



function populateTables() {
    populateUnselectedTable();
    populateSelectedTable();
    populateManagersList();
}

function populateUnselectedTable() {
    let unselectedGroupsTableBody = document.getElementById("groupsTableBody");
    unselectedGroupsTableBody.innerHTML = "";

    for (let gid in unselectedGroups) {
        let row = `
                <tr>
                    <td>${unselectedGroups[gid].gName}</td>
                    <td><button class="btn" type="button" onClick="addCollaborator(${gid});">Add</button></td>
                </tr>`;
        unselectedGroupsTableBody.innerHTML += row;
    }
}

function populateSelectedTable() {
    let selectedGroupsTableBody = document.getElementById("collaboratorsTableBody");
    selectedGroupsTableBody.innerHTML = "";

    for (let gid in selectedGroups) {
        let row = `
                <tr>
                    <td><button class="btn" type="button" onClick="removeCollaborator(${gid});">Remove</button></td>
                    <td>${selectedGroups[gid].gName}</td>
                </tr>`;
        selectedGroupsTableBody.innerHTML += row;
    }
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
    selectedGroups[id] = unselectedGroups[id];
    delete unselectedGroups[id];

    selectedManagerCount[selectedGroups[id].managerId]++;

    populateTables();
}

function removeCollaborator(id) {
    unselectedGroups[id] = selectedGroups[id];
    delete selectedGroups[id];
    selectedManagerCount[unselectedGroups[id].managerId]--;

    populateTables();
}



async function createProject() {
    let pLeadID = document.getElementById("projectLeadSelect").value;
    const projectData = {
        PTitle: document.getElementById("projectTitle").value.trim(),
        PDescription: document.getElementById("projectDescription").value.trim(),
        PLeadID: pLeadID ? pLeadID : 0,
        CollaboratingGroupIDs: Object.keys(selectedGroups)
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
            alert("Error: ${result.message}");
        }
    } catch (error) {
        alert("An error occurred while creating the group.");
    }
}