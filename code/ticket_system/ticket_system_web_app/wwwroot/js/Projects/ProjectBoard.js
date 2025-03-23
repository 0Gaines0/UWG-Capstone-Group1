   function openEditModal(btn) {
        document.getElementById('editTaskId').value = btn.dataset.taskId;
        document.getElementById('editTaskStateId').value = btn.dataset.stateId;
        document.getElementById('editTaskPriority').value = btn.dataset.priority;
        document.getElementById('editTaskSummary').value = btn.dataset.summary;
       document.getElementById('editTaskAssigneeId').value = btn.dataset.assigneeId || "";
       document.getElementById('editTaskDescription').value = btn.dataset.description;

        document.getElementById('editTaskModal').style.display = 'flex';
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

