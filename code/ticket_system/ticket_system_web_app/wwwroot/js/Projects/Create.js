document.addEventListener('DOMContentLoaded', async function () {
    fetchAllGroups();
});

let unselectedGroups = {}
let selectedGroups = {}

async function fetchAllGroups() {
    await fetch('/Groups/GetAllGroups').then(response => response.json()).then(data => {
        data.forEach(group => {
            unselectedGroups[group.gId] = group.gName;
        });
    }).catch(_error => alert('Error fetching groups:'));

    populateTables();
}

function populateTables() {
    let unselectedGroupsTableBody = document.getElementById('groupsTableBody');
    let selectedGroupsTableBody = document.getElementById('collaboratorsTableBody');
    unselectedGroupsTableBody.innerHTML = '';
    selectedGroupsTableBody.innerHTML = '';

    for (let gid in unselectedGroups) {
        let row = `
                <tr id="unselected-group-${gid}">
                    <td>${unselectedGroups[gid]}</td>
                    <td><button class="btn" type="button" onClick="addCollaborator(${gid});">Add</button></td>
                </tr>`;
        unselectedGroupsTableBody.innerHTML += row;
    }
    for (let gid in selectedGroups) {
        let row = `
                <tr id="selected-group-${gid}">
                    <td><button class="btn" type="button" onClick="removeCollaborator(${gid});">Remove</button></td>
                    <td>${selectedGroups[gid]}</td>
                </tr>`;
        selectedGroupsTableBody.innerHTML += row;
    }
}

function addCollaborator(id) {
    selectedGroups[id] = unselectedGroups[id];
    delete unselectedGroups[id];

    populateTables();
}

function removeCollaborator(id) {
    unselectedGroups[id] = selectedGroups[id];
    delete selectedGroups[id];

    populateTables();
}