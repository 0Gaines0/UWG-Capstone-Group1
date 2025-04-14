document.addEventListener("DOMContentLoaded", function () {
    authToken = document.getElementById("authToken").value;
    fetchProjects();
});

let authToken = "FAILED TO GET AUTH TOKEN";

function fetchProjects() {
    fetch(`/Projects/GetProjectRelatedToEmployee/${authToken}`)
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                renderProject(data.data);
            } else {
                console.error("Error fetching projects:", data.message);
            }
        }).catch(error => console.error("Error:", error));
}

function renderProject(projects) {
    const projectList = document.getElementById("projectList");
    projectList.innerHTML = "";

    if (projects.length === 0) {
        return;
    }

    projects.forEach(project => {
        const projectItem = document.createElement("div");
        projectItem.classList.add("project-item");
        projectItem.setAttribute("data-project-id", project.pId);
        projectItem.onclick = function () {
            openProjectBoard(this);
        };

        projectItem.innerHTML = `
                <img src="/assets/project_bookmark_img.png" alt="Bookmark">
                <span>${project.pTitle}</span>
            `;

        projectList.appendChild(projectItem);
    });
}

function openProjectBoard(element) {
    const projectId = element.getAttribute("data-project-id");
    console.log("Navigating to project with ID:", projectId);

    // Pass ID as a route parameter
    window.location.href = `/Projects/BoardPage/${projectId}`;
}