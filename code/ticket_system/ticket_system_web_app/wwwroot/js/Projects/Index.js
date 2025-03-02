let projectTitle = "";
let projectDescription = "";
let projectLead = "";
let projectCollaborators = [];

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

