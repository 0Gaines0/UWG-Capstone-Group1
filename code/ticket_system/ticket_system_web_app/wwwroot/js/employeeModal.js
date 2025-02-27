document.addEventListener('DOMContentLoaded', async function () {
    fetchEmployees();

    //await resetModal();

    //document.getElementById("managerSearch").addEventListener("input", filterManagers);
    //document.getElementById("employeeSearch").addEventListener("input", filterEmployees);
});

//let allManagers = [];
//let allEmployees = [];
//let selectedManager = "";
//let selectedMembers = [];

document.querySelector('#employeeTableBody').addEventListener('click', function (event) {
    if (event.target.closest('tr')) {
        let clickedRow = event.target.closest('tr');

        let rows = document.querySelectorAll('#employeeTableBody tr');
        rows.forEach(row => {
            if (row !== clickedRow) {
                row.classList.remove('selected');
            }
        });

        clickedRow.classList.toggle('selected');
    }
});

//function setupRows()
//    {
//        document.querySelectorAll('#employeeTableBody tr').forEach(row => {
//            row.addEventListener('click', function (event) {
//                let rows = document.querySelectorAll('#employeeTableBody tr');
//                rows.forEach(otherRow => {
//                    if (otherRow !== this) {
//                        otherRow.classList.remove('selected');
//                    }
//                });

//                this.classList.toggle('selected');
//            });
//        });
//    }

function fetchEmployees() {
    fetch('/Employees/GetAllEmployees')
        .then(response => response.json())
        .then(data => {
            let tableBody = document.getElementById('employeeTableBody');
            tableBody.innerHTML = '';

            data.forEach(employee => {
                let role = employee.isAdmin ? "Admin" : "Employee";
                let row = `
                    <tr>
                        <td>${employee.id}</td>
                        <td>${employee.name}</td>
                        <td>${employee.username}</td>
                        <td>${employee.email}</td>
                        <td>${role}</td>
                    </tr>`;
                tableBody.innerHTML += row;
            });
        })
        .catch(error => console.error('Error fetching groups:', error));
}

//async function fetchAllAvailableManagers() {
//    try {
//        const response = await fetch('/Groups/GetAllManagers');
//        const data = await response.json();
//        return data || [];
//    } catch (error) {
//        console.error('Error fetching Available Managers:', error);
//        return [];
//    }
//}

//async function fetchAllEmployees() {
//    try {
//        const response = await fetch('/Groups/GetAllEmployees');
//        const data = await response.json();
//        return data || [];
//    } catch (error) {
//        console.error('Error fetching Available Employees:', error);
//        return [];
//    }
//}

function openCreateModal() {
    document.getElementById("createEmployeeModal").style.display = "flex";
}

function closeCreateModal() {
    document.getElementById("createEmployeeModal").style.display = "none";
    //resetModal();
}

//async function resetModal() {
//    document.getElementById("groupName").value = "";
//    document.getElementById("groupDescription").value = "";
//    document.getElementById("managerSelect").innerHTML = `<option value="">Select a Manager</option>`;

//    clearErrors();

//    selectedManager = "";
//    selectedMembers = [];
//    allManagers = await fetchAllAvailableManagers();
//    allEmployees = await fetchAllEmployees();
//    populateLists();
//    populateManagerLists();
//}

//function validateForm() {
//    let isValid = true;

//    let nameInput = document.getElementById("groupName").value.trim();
//    let descriptionInput = document.getElementById("groupDescription").value.trim();
//    let managerInput = document.getElementById("managerSelect").value.trim();

//    clearErrors();

//    if (nameInput === "") {
//        displayError("groupNameError", "Group name is required.");
//        isValid = false;
//    }
//    if (descriptionInput === "") {
//        displayError("groupDescriptionError", "Description is required.");
//        isValid = false;
//    }
//    if (managerInput === "" || managerInput === "Select a Manager") {
//        displayError("managerSelectError", "Manager selection is required.");
//        isValid = false;
//    }

//    return isValid;
//}

//function displayError(errorId, message) {
//    let errorLabel = document.getElementById(errorId);
//    errorLabel.innerText = message;
//    errorLabel.style.display = "block";
//}

//function clearErrors() {
//    document.querySelectorAll(".error-message").forEach(error => {
//        error.innerText = "";
//        error.style.display = "none";
//    });
//}

async function createEmployee() {
    //TODO
    //if (!validateForm()) return;

    const employeeData = {
        FirstName: document.getElementById("firstName").value.trim(),
        LastName: document.getElementById("lastName").value.trim(),
        UserName: document.getElementById("username").value.trim(),
        Password: document.getElementById("password").value.trim(),
        Email: document.getElementById("email").value.trim(),
        IsAdmin: document.getElementById("isAdmin").checked
    };

    console.log("Sending Employee Data:", employeeData);

    try {
        const response = await fetch("/Employees/CreateEmployee", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(employeeData)
        });

        const result = await response.json();

        console.log("Response Received:", result);
        if (response.ok) {
            alert("Employee created successfully!");
            closeCreateModal();
            fetchEmployees();
        } else {
            alert(`Error: ${result.message}`);
        }
    } catch (error) {
        console.error("Error creating employee:", error);
        alert("An error occurred while creating the employee.");
    }
}

//function populateLists() {
//    document.getElementById("employeeList").innerHTML = allEmployees
//        .map(employee => `<p>${employee.name} <button class="select-btn" onclick="addMember(${employee.id}, '${employee.name}')">+</button></p>`)
//        .join("");

//    document.getElementById("selectedEmployees").innerHTML = selectedMembers.length
//        ? selectedMembers.map(member => `<p>${member.name} <button class="remove-btn" onclick="removeMember(${member.id})">-</button></p>`).join("")
//        : "<p>No members added</p>";
//}

//function selectManager(id, name) {
//    if (selectedManager) {
//        allManagers.push(selectedManager);
//    }

//    selectedManager = { id, name };

//    document.getElementById("managerSelect").innerHTML = `
//        <option value="">Select a Manager</option>
//        <option value="${id}" selected>${name}</option>
//    `;

//    allManagers = allManagers.filter(m => m.id !== id);
//    document.getElementById("removeManagerBtn").style.display = "inline-block";
//    populateManagerLists();
//}

//function removeManager() {
//    if (selectedManager) {
//        allManagers.push(selectedManager);
//        selectedManager = null;

//        document.getElementById("managerSelect").innerHTML = `<option value="">Select a Manager</option>`;

//        document.getElementById("removeManagerBtn").style.display = "none";
//        populateManagerLists();
//    }
//}

//function populateManagerLists() {
//    let managerDropdown = document.getElementById("managerSelect");
//    let managerList = document.getElementById("managerList");

//    managerDropdown.innerHTML = `<option value="">Select a Manager</option>`;
//    managerList.innerHTML = "";

//    allManagers.forEach(manager => {
//        managerList.innerHTML += `<p>${manager.name}
//            <button class="select-btn" onclick="selectManager(${manager.id}, '${manager.name}')">+</button>
//        </p>`;
//    });

//    if (selectedManager) {
//        managerDropdown.innerHTML = `<option value="${selectedManager.id}">${selectedManager.name}</option>`;
//    }
//}

//function addMember(id, name) {
//    if (!selectedMembers.some(m => m.id === id)) {
//        selectedMembers.push({ id, name });
//        allEmployees = allEmployees.filter(e => e.id !== id);
//        populateLists();
//    }
//}

//function removeMember(id) {
//    let member = selectedMembers.find(m => m.id === id);
//    selectedMembers = selectedMembers.filter(m => m.id !== id);
//    if (member) allEmployees.push(member); // Restore removed member
//    populateLists();
//}

//function filterManagers() {
//    let searchValue = document.getElementById("managerSearch").value.toLowerCase();
//    let managerItems = document.querySelectorAll("#managerList p");

//    managerItems.forEach((item) => {
//        let managerName = item.textContent.toLowerCase();
//        if (managerName.includes(searchValue)) {
//            item.style.display = "flex";
//        } else {
//            item.style.display = "none";
//        }
//    });
//}

//function filterEmployees() {
//    let searchValue = document.getElementById("employeeSearch").value.toLowerCase();
//    let employeeItems = document.querySelectorAll("#employeeList p");

//    employeeItems.forEach((item) => {
//        let employeeName = item.textContent.toLowerCase();
//        if (employeeName.includes(searchValue)) {
//            item.style.display = "flex";
//        } else {
//            item.style.display = "none";
//        }
//    });
//}

//function openRemoveModal() {
//    document.getElementById("removeGroupModal").style.display = "flex";
//}

//function closeRemoveModal() {
//    document.getElementById("removeGroupModal").style.display = "none";
//    document.getElementById("removeGroupName").value = "";
//    document.getElementById("removeGroupNameError").style.display = "none";
//    document.getElementById("removeGroupBtn").disabled = true;
//}

//function validateRemoveGroup() {
//    const groupNameInput = document.getElementById("removeGroupName").value.trim();
//    const errorLabel = document.getElementById("removeGroupNameError");
//    const removeBtn = document.getElementById("removeGroupBtn");

//    if (groupNameInput === "") {
//        errorLabel.style.display = "block";
//        removeBtn.disabled = true;
//    } else {
//        errorLabel.style.display = "none";
//        removeBtn.disabled = false;
//    }
//}

function removeEmployee() {
    let rows = Array.from(document.querySelectorAll('#employeeTableBody tr'));
    let selectedRow;
    rows.some(row => {
        if (row.classList.contains('selected')) {
            selectedRow = row;
            return;
        }
    });
    if (selectedRow == null) {
        return;
        //TODO: Add error
    }
    const employeeId = parseInt(selectedRow.cells[0].textContent);

    fetch("/Employees/RemoveEmployee", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ employeeId })
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                alert("User removed successfully!");
                fetchEmployees();
            } else {
                alert("Error: " + data.message);
            }
        })
        .catch(error => {
            console.error("Error removing user:", error);
            alert("An error occurred.");
        });
}

function filterEmployees() {
    let searchValue = document.getElementById("employeeSearch").value.toLowerCase();
    let rows = document.querySelectorAll("#employeeTableBody tr");

    rows.forEach(row => {
        let employeeName = row.querySelector("td:nth-child(2)").textContent.toLowerCase();
        if (employeeName.includes(searchValue)) {
            row.style.display = ""; // Show row
        } else {
            row.style.display = "none"; // Hide row
            row.classList.remove('selected');
        }
    });
}


