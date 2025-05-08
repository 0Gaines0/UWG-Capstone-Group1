document.addEventListener("DOMContentLoaded", async function () {
    authToken = document.getElementById("authToken").value;
});

let authToken = "FAILED TO GET AUTH TOKEN";

function openEditModal(btn) {
    const taskId = btn.dataset.taskId;
    const projectId = btn.dataset.projectId;

    document.getElementById('editTaskId').value = taskId;
    document.getElementById('editTaskStateId').value = btn.dataset.stateId;
    document.getElementById('editTaskPriority').value = btn.dataset.priority;
    document.getElementById('editTaskSummary').value = btn.dataset.summary;
    document.getElementById('editTaskDescription').value = btn.dataset.description;

    populateEmployeesByState(btn.dataset.assigneeId || "", document.getElementById("projectIdHolder").value);
    document.getElementById('editTaskAssigneeId').value = btn.dataset.assigneeId || "";

    document.getElementById('edit-task').style.display = "flex";
    document.getElementById('create-task').style.display = "none";
    document.getElementById('comments-history').style.display = "block";
    document.getElementById('modal-title').textContent = "Edit Task";
    document.getElementById('editTaskModal').style.display = 'flex';

    loadTaskHistory(taskId);
    fetch(`/Task/Comments/${taskId}`)
        .then(res => res.json())
        .then(comments => {
            const ul = document.querySelector("#tab-comments ul");
            ul.innerHTML = "";
            for (const comment of comments) {
                const li = document.createElement("li");
                li.textContent = `${comment.commenter}: ${comment.text} (${comment.date})`;
                ul.appendChild(li);
            }
        })
        .catch(err => {
            console.error("Failed to load comments:", err);
        });
}


function openCreateTaskModal() {
    const projectId = document.getElementById('projectIdHolder').value;

    document.getElementById('editTaskStateId').value = document.getElementById('editTaskStateId').options[0].value;
    document.getElementById('editTaskPriority').value = 1;
    document.getElementById('editTaskSummary').value = "";
    document.getElementById('editTaskDescription').value = "";

    populateEmployeesByState("", document.getElementById("projectIdHolder").value);
    document.getElementById('editTaskAssigneeId').value = "";

    document.getElementById('comments-history').style.display = "none";
    document.getElementById('edit-task').style.display = "none";
    document.getElementById('create-task').style.display = "flex";
    document.getElementById('modal-title').textContent = "Create Task";

    document.getElementById('editTaskModal').style.display = 'flex';
}


    function closeModal() {
        document.getElementById('editTaskModal').style.display = 'none';
    }

async function submitCreateTask() {
    const form = document.getElementById('editTaskForm');
    const formData = new FormData(form);
    const response = await fetch(`/Task/Create/${authToken}`, {
        method: 'POST',
        body: formData
    });

    if (response.ok) {
        window.location.reload();
    } else {
        alert("Failed to create task.");
    }
}

    async function submitEditTask() {
        const form = document.getElementById('editTaskForm');
        const formData = new FormData(form);
        const response = await fetch(`/Task/Edit/${authToken}`, {
            method: 'POST',
            body: formData
        });

        if (response.ok) {
            window.location.reload();
        } else {
            alert("Failed to update task.");
        }
    }

    async function confirmDeleteTask(btn) {
        const taskId = btn.dataset.taskId;

        const confirmed = confirm(`Are you sure you want to delete Task #${taskId}?`);
        if (!confirmed) return;

        const response = await fetch(`/Task/Delete/${authToken}&${taskId}`, {
            method: 'DELETE'
        });

        if (response.ok) {
            window.location.reload();
        } else {
            alert("Failed to delete task.");
        }
}

function switchTab(tabName) {
    const tabButtons = document.querySelectorAll('.tab-button');
    tabButtons.forEach(btn => btn.classList.remove('active'));

    const tabContents = document.querySelectorAll('.tab-content');
    tabContents.forEach(content => content.classList.remove('active'));

    document.querySelector(`.tab-button[onclick*="'${tabName}'"]`).classList.add('active');
    document.getElementById(`tab-${tabName}`).classList.add('active');
}
async function loadTaskHistory(taskId) {
    const response = await fetch(`/Task/History/${authToken}&${taskId}`);
    if (!response.ok) {
        console.error("Failed to fetch history");
        return;
    }

    const history = await response.json();
    const tbody = document.getElementById('historyTableBody');

    if (!tbody) {
        console.warn("Element #historyTableBody not found");
        return;
    }

    tbody.innerHTML = ''; 

    if (history.length === 0) {
        const row = document.createElement('tr');
        const cell = document.createElement('td');
        cell.colSpan = 5;
        cell.textContent = 'No history found.';
        row.appendChild(cell);
        tbody.appendChild(row);
        return;
    }

    history.forEach(change => {
        const row = document.createElement('tr');

        row.innerHTML = `
            <td>${new Date(change.changedDate).toLocaleString()}</td>
            <td>${change.type}</td>
            <td>${change.previousValue || '(none)'}</td>
            <td>${change.newValue || '(none)'}</td>
            <td>${change.assigneeName || 'Unassigned'}</td>
        `;

        tbody.appendChild(row);
    });
}


function submitComment(event) {
    event.preventDefault();

    const commentText = document.getElementById('newCommentText').value.trim();
    if (!commentText) {
        alert("Please enter a comment.");
        return;
    }

    const authToken = document.getElementById("authToken").value;
    const taskId = document.getElementById("editTaskId").value;

    fetch(`/Task/AddComment/${authToken}&${taskId}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: new URLSearchParams({ commentText })
    })
        .then(response => {
            if (!response.ok) throw new Error("Failed to submit comment.");
            return response.json();
        })
        .then(() => {
            document.getElementById('newCommentText').value = "";

            return fetch(`/Task/Comments/${taskId}`);
        })
        .then(res => res.json())
        .then(comments => {
            const ul = document.querySelector("#tab-comments ul");
            ul.innerHTML = "";
            for (const comment of comments) {
                const li = document.createElement("li");
                li.textContent = `${comment.commenter}: ${comment.text} (${comment.date})`;
                ul.appendChild(li);
            }
        })
        .catch(err => {
            console.error(err);
            alert("Failed to submit comment.");
        });
}

async function populateEmployeesByState(selectedAssigneeId = "", projectId = null) {
    selectedAssigneeId = selectedAssigneeId?.toString() ?? "";

    const stateSelect = document.getElementById('editTaskStateId');
    const employeeSelect = document.getElementById('editTaskAssigneeId');
    const stateId = parseInt(stateSelect.value);

    employeeSelect.innerHTML = '<option value="">Unassigned</option>';

    let employeesToUse = [];
    let matchFound = false;

    if (projectId) {
        try {
            const response = await fetch(`/Projects/GetProjectEmployees/${authToken}&${projectId}&${stateId}`);
            if (response.ok) {
                employeesToUse = await response.json();
            } else {
                console.warn("Fallback failed to load project employees");
            }
        } catch (error) {
            console.error("Error fetching fallback employees:", error);
        }
    }

    employeesToUse.forEach(emp => {
        const id = emp.EId ?? emp.eId;
        const name = emp.Name ?? emp.name;

        if (!id || !name) {
            console.warn("Skipping invalid employee entry:", emp);
            return;
        }

        const option = document.createElement("option");
        option.value = id;
        option.textContent = name;

        if (id.toString() === selectedAssigneeId?.toString()) {
            option.selected = true;
            matchFound = true;
        }

        employeeSelect.appendChild(option);
    });

    if (!matchFound) {
        const unassignedOption = employeeSelect.querySelector('option[value=""]');
        if (unassignedOption) {
            unassignedOption.selected = true;
        }
    }
}



document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('editTaskStateId').addEventListener('change', () => {
        const currentAssigneeId = document.getElementById('editTaskAssigneeId').value || "";
        const projectId = document.getElementById('projectIdHolder').value;
        populateEmployeesByState(currentAssigneeId, projectId);
    });
});





