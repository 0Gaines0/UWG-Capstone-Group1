document.addEventListener('DOMContentLoaded', async function () {
    alert("UWU");
});

let projectTitle = "";
let projectDescription = "";
let projectLead = "";
let projectCollaborators = [];

async function viewProject(id) {
    alert("UWU");
    await retrieveProject(id);
    displayProject();
}

async function retrieveProject(id) {
    await fetch(`/Projects/Details/${id}`, {
        method: "POST"
    }).then(response => response.json()).then(data => {
        projectTitle = data.pTitle;
        projectDescription = data.pDescription;
        projectLead = data.projectLeadName
        projectCollaborators = data.assignedGroups
    }).catch(_error => alert("Error fetching project."));
}

function displayProject() {
    document.getElementById("projectTitle").value = projectTitle;
    document.getElementById("projectDescription").value = projectDescription;
    document.getElementById("projectLead").value = projectLead;

    let tableBody = document.getElementById("projectCollaborators").innerHTML = "";
    projectCollaborators.forEach(group => {
        alert(group.gName);
        let row = `
            <tr>
                <td>
                    ${group.gName}
                </td>
            </tr`;
        tableBody.innerHTML += row;
    });
}
