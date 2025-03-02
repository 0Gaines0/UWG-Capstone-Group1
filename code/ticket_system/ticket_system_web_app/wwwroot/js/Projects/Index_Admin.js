// MUST ALWAYS BE LOADED AFTER Index.js
    //Relies on viewProject(id) function

let deletingProjectId = -1;

function promptDeleteProject(id) {
    viewProject(id);
    deletingProjectId = id;
    document.getElementById("projects-list-content").style.display = "none";
    document.getElementById("delete-prompt-content").style.display = "flex";
}

function cancelDeleteProject() {
    deletingProjectId = -1;
    document.getElementById("projects-list-content").style.display = "table";
    document.getElementById("delete-prompt-content").style.display = "none";
}

function confirmDeleteProject() {
    document.getElementById(`delete-row-${deletingProjectId}`).click();
}