﻿function openEditModal(btn) {

        document.getElementById('editTaskId').value = btn.dataset.taskId;
        document.getElementById('editTaskStateId').value = btn.dataset.stateId;
        document.getElementById('editTaskPriority').value = btn.dataset.priority;
        document.getElementById('editTaskSummary').value = btn.dataset.summary;
       document.getElementById('editTaskAssigneeId').value = btn.dataset.assigneeId || "";
       document.getElementById('editTaskDescription').value = btn.dataset.description;

    document.getElementById('editTaskModal').style.display = 'flex';
    loadTaskHistory(btn.dataset.taskId);

    }

    function closeModal() {
        document.getElementById('editTaskModal').style.display = 'none';
    }

    async function submitEditTask() {
        const form = document.getElementById('editTaskForm');
        const formData = new FormData(form);
        const response = await fetch('/Task/Edit', {
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

        const response = await fetch(`/Task/Delete/${taskId}`, {
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
    const response = await fetch(`/Task/History/${taskId}`);
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
    console.log("Submitting comment:", commentText);
    document.getElementById('newCommentText').value = "";
}


