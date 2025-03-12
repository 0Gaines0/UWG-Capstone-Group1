document.addEventListener('DOMContentLoaded', function () {
    fetchEmployees();
});

// functionlity for selecting a employee by clicking them 
document.getElementById('employeeTableBody').addEventListener('click', function (event) {
    if (event.target.closest('tr')) {
        let clickedRow = event.target.closest('tr');

        let rows = document.querySelectorAll('#employeeTableBody tr');
        rows.forEach(row => {
            if (row !== clickedRow) {
                row.classList.remove('selected');
            }
        });

        clickedRow.classList.toggle('selected');
        if (clickedRow.classList.contains('selected')) {
            selectedEmployee();
        } else {
            noSelectedEmployee();
        }
    }
});

var selectedUsername = "";

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

function selectedEmployee() {
    document.getElementById("remove-employee-btn").removeAttribute('disabled');
    document.getElementById("remove-employee-btn").title = "";
    document.getElementById("edit-employee-btn").removeAttribute('disabled');
    document.getElementById("edit-employee-btn").title = "";
}

function noSelectedEmployee() {
    document.getElementById("remove-employee-btn").setAttribute('disabled', true);
    document.getElementById("remove-employee-btn").title = "Select a employee to remove them";
    document.getElementById("edit-employee-btn").setAttribute('disabled', true);
    document.getElementById("edit-employee-btn").title = "Select a employee to edit them";
}
function openEditModal() {
    resetModal();
    //Gets selected employee
    let rows = Array.from(document.querySelectorAll('#employeeTableBody tr'));
    let selectedRow;
    rows.some(row => {
        if (row.classList.contains('selected')) {
            selectedRow = row;
            return;
        }
    });
    if (selectedRow == null) {
        alert("No employee selected for editing.");
        return;
    }
    const username = selectedRow.cells[1].textContent;
    selectedUsername = username;

    fetch('/Employees/GetEmployee', {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ username })
    })
        .then(response => response.json())
        .then(data => {
            document.getElementById("firstName").value = data.fName;
            document.getElementById("lastName").value = data.lName;
            document.getElementById("username").value = data.username;
            document.getElementById("email").value = data.email;
            document.getElementById("isAdmin").checked = data.isAdmin;
        })
        .catch(error => console.error('Error fetching employee:', error));
    document.getElementById("modalTitle").textContent = "Edit Employee";
    document.getElementById("passwordField").style.display = "none";
    document.getElementById("create-btn").style.display = "none";
    document.getElementById("edit-btn").style.display = "flex";
    document.getElementById("createEmployeeModal").style.display = "flex";
}

function openCreateModal() {
    resetModal();
    document.getElementById("modalTitle").textContent = "Create Employee";
    document.getElementById("passwordField").style.display = "block";
    document.getElementById("create-btn").style.display = "flex";
    document.getElementById("edit-btn").style.display = "none";
    document.getElementById("createEmployeeModal").style.display = "flex";
}

function closeModal() {
    document.getElementById("createEmployeeModal").style.display = "none";
}

async function resetModal() {
    document.getElementById("firstName").value = "";
    document.getElementById("lastName").value = "";
    document.getElementById("username").value = "";
    document.getElementById("password").value = "";
    document.getElementById("email").value = "";
    document.getElementById("isAdmin").checked = false;

    clearErrors();
}

/**
 * Returns true if the form is valid, and adds error messages for incorrect inputs.
 * 
 * @param {any} formData the data in the form
 * @returns true if the form is valid and false otherwise
 */
function validateFormForCreatingEmployee(formData) {
    let isValid = true;

    clearErrors();

    if (formData.FirstName === "") {
        displayError("firstNameError", "First name is required.");
        isValid = false;
    }
    if (formData.LastName === "") {
        displayError("lastNameError", "Last name is required.");
        isValid = false;
    }
    if (formData.UserName === "") {
        displayError("usernameError", "Username is required.");
        isValid = false;
    }
    if (formData.Password === "") {
        displayError("passwordError", "Password is required.");
        isValid = false;
    }
    if (formData.Email === "") {
        displayError("emailError", "Email is required.");
        isValid = false;
    }

    return isValid;
}

/**
 * Returns true if the form is valid, and adds error messages for incorrect inputs.
 * 
 * @param {any} formData the data in the form
 * @returns true if the form is valid and false otherwise
 */
function validateFormForEditingEmployees(formData) {
    let isValid = true;

    clearErrors();

    if (formData.FirstName === "") {
        displayError("firstNameError", "First name is required.");
        isValid = false;
    }
    if (formData.LastName === "") {
        displayError("lastNameError", "Last name is required.");
        isValid = false;
    }
    if (formData.UserName === "") {
        displayError("usernameError", "Username is required.");
        isValid = false;
    }
    if (formData.Email === "") {
        displayError("emailError", "Email is required.");
        isValid = false;
    }

    return isValid;
}

function displayError(errorId, message) {
    let errorLabel = document.getElementById(errorId);
    errorLabel.innerText = message;
    errorLabel.style.display = "block";
}

function clearErrors() {
    document.querySelectorAll(".error-message").forEach(error => {
        error.innerText = "";
        error.style.display = "none";
    });
}

async function createEmployee() {
    const employeeData = {
        FirstName: document.getElementById("firstName").value.trim(),
        LastName: document.getElementById("lastName").value.trim(),
        UserName: document.getElementById("username").value.trim(),
        Password: document.getElementById("password").value.trim(),
        Email: document.getElementById("email").value.trim(),
        IsAdmin: document.getElementById("isAdmin").checked
    };

    if (!validateFormForCreatingEmployee(employeeData)) return;

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
            closeModal();
            fetchEmployees();
        } else {
            alert(`Error: ${result.message}`);
        }
    } catch (error) {
        console.error("Error creating employee:", error);
        alert("An error occurred while creating the employee.");
    }
}

async function editEmployee() {
    const employeeData = {
        OriginalUsername: selectedUsername,
        FirstName: document.getElementById("firstName").value.trim(),
        LastName: document.getElementById("lastName").value.trim(),
        UserName: document.getElementById("username").value.trim(),
        Email: document.getElementById("email").value.trim(),
        IsAdmin: document.getElementById("isAdmin").checked
    };

    if (!validateFormForEditingEmployees(employeeData)) return;

    try {
        const response = await fetch("/Employees/EditEmployee", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(employeeData)
        });

        const result = await response.json();

        console.log("Response Received:", result);
        if (response.ok) {
            alert("Employee updated successfully!");
            closeModal();
            fetchEmployees();
        } else {
            alert(`Error: ${result.message}`);
        }
    } catch (error) {
        console.error("Error updating employee:", error);
        alert("An error occurred while updating the employee.");
    }
}

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
        alert("No employee selected for removal.");
        return;
    }
    const username = selectedRow.cells[1].textContent;

    fetch("/Employees/RemoveEmployee", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ username })
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                alert("User removed successfully!");
                fetchEmployees();
                noSelectedEmployee();
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


