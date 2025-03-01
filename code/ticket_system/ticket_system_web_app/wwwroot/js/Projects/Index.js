let projectTitle = "";
let projectDescription = "";
let projectLead = "";
let projectCollaborators = [];

let deletingProjectId = -1;

async function viewProject(id) {
    await retrieveProject(id);
    displayProject();
}

async function retrieveProject(id) {
    await fetch(`/Projects/Details/${id}`, {
        method: "POST"
    }).then(response => response.json()).then(data => {
        projectTitle = data.pTitle;
        projectDescription = data.pDescription;
        projectLead = data.projectLeadName;
        projectCollaborators = data.assignedGroups;
    }).catch(_error => alert("Error fetching project."));
}

function displayProject() {
    document.getElementById("projectTitle").value = projectTitle;
    document.getElementById("projectDescription").value = projectDescription;
    document.getElementById("projectLead").value = projectLead;

    let tableBody = document.getElementById("projectCollaborators");
    tableBody.innerHTML = ""
    projectCollaborators.forEach(group => {
        let row = `
            <tr>
                <td>
                    ${group.gName}
                </td>
            </tr`;
        tableBody.innerHTML += row;
    });
}

async function promptDeleteProject(id) {
    viewProject(id);
    deletingProjectId = id;
    document.getElementById("projects-list-content").style.display = "none";
    document.getElementById("delete-prompt-content").style.display = "flex";
}

function cancelDeleteProject() {
    deletingProjectId = -1;
    document.getElementById("projects-list-content").style.display = "block";
    document.getElementById("delete-prompt-content").style.display = "none";
}

function confirmDeleteProject() {
    document.getElementById(`delete-row-${deletingProjectId}`).click();
}