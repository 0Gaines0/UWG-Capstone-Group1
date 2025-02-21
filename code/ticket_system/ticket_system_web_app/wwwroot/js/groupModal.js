document.addEventListener('DOMContentLoaded', async function () {
    fetchGroups();
    fetchUserGroups();
    await resetModal();

    document.getElementById("managerSearch").addEventListener("input", filterManagers);
    document.getElementById("employeeSearch").addEventListener("input", filterEmployees);
});

let allManagers = [];
let allEmployees = [];
let selectedManager = "";
let selectedMembers = [];


function fetchGroups() {
    fetch('/Groups/GetAllGroups')
        .then(response => response.json())
        .then(data => {
            let tableBody = document.getElementById('groupTableBody');
            tableBody.innerHTML = '';

            data.forEach(group => {
                let row = `
                    <tr>
                        <td>${group.gName}</td>
                        <td>${group.managerName || 'Not Assigned'}</td>
                        <td>${group.membersCount}</td>
                        <td>${group.gDescription}</td>
                    </tr>`;
                tableBody.innerHTML += row;
            });
        })
        .catch(error => console.error('Error fetching groups:', error));
}

function fetchUserGroups() {
    fetch('/Groups/GetActiveUserGroups')
        .then(response => response.json())
        .then(data => {
            let list = document.getElementById('myGroupItem');
            list.innerHTML = '';

            data.forEach(groupName => {
                let subItem = `
                    <div class='my-groups-item'>
                        <img src='/assets/groups_icon_img.png' alt='Group Icon'>
                        <span>${groupName}</span>
                    </div>`;
                list.innerHTML += subItem;
            });
        })
        .catch(error => console.error('Error fetching my groups:', error));
}

async function fetchAllAvailableManagers() {
    try {
        const response = await fetch('/Groups/GetAllManagers');
        const data = await response.json();
        return data || [];
    } catch (error) {
        console.error('Error fetching Available Managers:', error);
        return [];
    }
}

async function fetchAllEmployees() {
    try {
        const response = await fetch('/Groups/GetAllEmployees');
        const data = await response.json();
        return data || [];
    } catch (error) {
        console.error('Error fetching Available Employees:', error);
        return [];
    }
}

function openCreateModal() {
    document.getElementById("createGroupModal").style.display = "flex";
}

function closeCreateModal() {
    document.getElementById("createGroupModal").style.display = "none";
    resetModal();
}

async function resetModal() {
    document.getElementById("groupName").value = "";
    document.getElementById("groupDescription").value = "";
    document.getElementById("managerSelect").innerHTML = `<option value="">Select a Manager</option>`;

    clearErrors();

    selectedManager = "";
    selectedMembers = [];
    allManagers = await fetchAllAvailableManagers(); 
    allEmployees = await fetchAllEmployees();
    populateLists();
    populateManagerLists();
}

function validateForm() {
    let isValid = true;

    let nameInput = document.getElementById("groupName").value.trim();
    let descriptionInput = document.getElementById("groupDescription").value.trim();
    let managerInput = document.getElementById("managerSelect").value.trim();

    clearErrors();

    if (nameInput === "") {
        displayError("groupNameError", "Group name is required.");
        isValid = false;
    }
    if (descriptionInput === "") {
        displayError("groupDescriptionError", "Description is required.");
        isValid = false;
    }
    if (managerInput === "" || managerInput === "Select a Manager") {
        displayError("managerSelectError", "Manager selection is required.");
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

function createGroup() {
    if (validateForm()) {
        closeCreateModal();
    }
}

function populateLists() {
    document.getElementById("employeeList").innerHTML = allEmployees
        .map(employee => `<p>${employee.name} <button class="select-btn" onclick="addMember(${employee.id}, '${employee.name}')">+</button></p>`)
        .join("");

    document.getElementById("selectedEmployees").innerHTML = selectedMembers.length
        ? selectedMembers.map(member => `<p>${member.name} <button class="remove-btn" onclick="removeMember(${member.id})">-</button></p>`).join("")
        : "<p>No members added</p>";
}

function selectManager(id, name) {
    if (selectedManager) {
        allManagers.push(selectedManager); 
    }

    selectedManager = { id, name };
    document.getElementById("managerSelect").innerHTML = `<option value="${id}">${name}</option>`;

    allManagers = allManagers.filter(m => m.id !== id);

    document.getElementById("removeManagerBtn").style.display = "inline-block";

    populateManagerLists();
}



function removeManager() {
    if (selectedManager) {
        allManagers.push(selectedManager); 
        selectedManager = null;
        document.getElementById("managerSelect").innerHTML = `<option value="">Select a Manager</option>`; 
        document.getElementById("removeManagerBtn").style.display = "none"; 
        populateManagerLists(); 
    }
}


function populateManagerLists() {
    let managerDropdown = document.getElementById("managerSelect");
    let managerList = document.getElementById("managerList");

    managerDropdown.innerHTML = `<option value="">Select a Manager</option>`;
    managerList.innerHTML = "";

    allManagers.forEach(manager => {
        managerDropdown.innerHTML += `<option value="${manager.id}">${manager.name}</option>`;
        managerList.innerHTML += `<p>${manager.name} 
            <button class="select-btn" onclick="selectManager(${manager.id}, '${manager.name}')">+</button>
        </p>`;
    });

    if (selectedManager) {
        managerDropdown.innerHTML = `<option value="${selectedManager.id}">${selectedManager.name}</option>`;
    }
}




function addMember(id, name) {
    if (!selectedMembers.some(m => m.id === id)) {
        selectedMembers.push({ id, name });
        allEmployees = allEmployees.filter(e => e.id !== id);
        populateLists();
    }
}

function removeMember(id) {
    let member = selectedMembers.find(m => m.id === id);
    selectedMembers = selectedMembers.filter(m => m.id !== id);
    if (member) allEmployees.push(member); // Restore removed member
    populateLists();
}

function filterManagers() {
    let searchValue = document.getElementById("managerSearch").value.toLowerCase();
    let managerItems = document.querySelectorAll("#managerList p");

    managerItems.forEach((item) => {
        let managerName = item.textContent.toLowerCase();
        if (managerName.includes(searchValue)) {
            item.style.display = "flex";
        } else {
            item.style.display = "none";
        }
    });
}

function filterEmployees() {
    let searchValue = document.getElementById("employeeSearch").value.toLowerCase();
    let employeeItems = document.querySelectorAll("#employeeList p");

    employeeItems.forEach((item) => {
        let employeeName = item.textContent.toLowerCase();
        if (employeeName.includes(searchValue)) {
            item.style.display = "flex";
        } else {
            item.style.display = "none";
        }
    });
}



