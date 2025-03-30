document.addEventListener('DOMContentLoaded', async function () {
    authToken = document.getElementById("authToken").value;   
    alert(authToken);

    fetchGroups();
    fetchUserGroups();
    await resetModal();

    document.getElementById("managerSearch").addEventListener("input", filterManagers);
    document.getElementById("employeeSearch").addEventListener("input", filterEmployees);

    document.getElementById("editManagerSearch").addEventListener("input", filterEditManagers);
    document.getElementById("editEmployeeSearch").addEventListener("input", filterEditEmployees);

  

    document.addEventListener("click", function (event) {
        let table = document.getElementById("groupTableBody");

        if (table && !table.contains(event.target)) {
            console.log("Clicked outside the table, deselecting row.");
            document.querySelectorAll("#groupTableBody tr").forEach(r => r.classList.remove("selected-row"));

            selectedGroupId = null;
        }
    });
});

let allManagers = [];
let allEmployees = [];
let selectedManager = "";
let selectedMembers = [];
let selectedGroupId = null;
let modalGroupId = null;

let allEditManagers = [];
let allEditEmployees = [];
let selectedEditManager = "";
let selectedEditMembers = [];
let selectedEditGroupId = null;

let authToken = "FAILED TO GET AUTH TOKEN";

function fetchGroups() {
    fetch(`/Groups/GetAllGroups/${authToken}`)
        .then(response => response.json())
        .then(data => {
            let tableBody = document.getElementById('groupTableBody');
            tableBody.innerHTML = '';

            data.forEach(group => {
                let row = document.createElement('tr');
                row.setAttribute('data-group-id', group.gId); 
                row.setAttribute('onclick', 'selectGroup(this)'); 

                row.innerHTML = `
                    <td>${group.gName}</td>
                    <td>${group.managerName || 'Not Assigned'}</td>
                    <td>${group.membersCount}</td>
                    <td>${group.gDescription}</td>
                `;

                tableBody.appendChild(row);

            });
        })
        .catch(error => console.error('Error fetching groups:', error));
}

function fetchUserGroups() {
    fetch(`/Groups/GetActiveUserGroups/${authToken}`)
        .then(response => response.json())
        .then(data => {
            if (!data || !Array.isArray(data)) { 
                console.error("Error: Received invalid group data", data);
                return;
            }

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
        const response = await fetch(`/Groups/GetAllManagers/${authToken}`);
        const data = await response.json();
        return data || [];
    } catch (error) {
        console.error('Error fetching Available Managers:', error);
        return [];
    }
}

async function fetchAllEmployees() {
    try {
        const response = await fetch(`/Groups/GetAllEmployees/${authToken}`);
        const data = await response.json();
        return data || [];
    } catch (error) {
        console.error('Error fetching Available Employees:', error);
        return [];
    }
}

function openCreateModal() {
    document.getElementById("createGroupModal").style.display = "flex";
    console.log(document.activeElement)
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
    let managerInput = document.getElementById("managerSelect").value.trim();

    clearErrors();

    if (nameInput === "") {
        displayError("groupNameError", "Group name is required.");
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

async function createGroup() {
    if (!validateForm()) return;
  
    const groupData = {
        groupName: document.getElementById("groupName").value.trim(),
        groupDescription: document.getElementById("groupDescription").value.trim(),
        managerId: selectedManager ? selectedManager.id : 0,
        memberIds: selectedMembers.map(member => member.id)
    };

    console.log("Sending Group Data:", groupData);

    try {
        const response = await fetch(`/Groups/CreateGroup/${authToken}`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(groupData)
        });

        const result = await response.json();

        console.log("Response Received:", result);
        if (response.ok) {
            showToast("Group created successfully!");
            closeCreateModal();
            fetchUserGroups();
            fetchGroups();
        } else {
            showToast(`Error: ${result.message}`);

        }
    } catch (error) {
        console.error("Error creating group:", error);
        showToast("An error occurred while creating the group.");
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

    document.getElementById("managerSelect").innerHTML = `
        <option value="">Select a Manager</option>
        <option value="${id}" selected>${name}</option>
    `;

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
    if (member) allEmployees.push(member); 
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

function openRemoveModal() {
    document.getElementById("removeGroupModal").style.display = "flex";
}

function closeRemoveModal() {
    document.getElementById("removeGroupModal").style.display = "none";
    document.getElementById("removeGroupName").value = "";
    document.getElementById("removeGroupNameError").style.display = "none";
    document.getElementById("removeGroupBtn").disabled = true;
}

function validateRemoveGroup() {
    const groupNameInput = document.getElementById("removeGroupName").value.trim();
    const errorLabel = document.getElementById("removeGroupNameError");
    const removeBtn = document.getElementById("removeGroupBtn");

    if (groupNameInput === "") {
        errorLabel.style.display = "block";
        removeBtn.disabled = true;
    } else {
        errorLabel.style.display = "none";
        removeBtn.disabled = false;
    }
}

function removeGroup() {
    const groupName = document.getElementById("removeGroupName").value.trim();

    if (groupName === "") {
        showToast("Please enter a valid group name.");
        return;
    }

    fetch(`/Groups/RemoveGroup/${authToken}`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ groupName })
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                showToast("Group removed successfully!");
                closeRemoveModal();
                fetchUserGroups();
                fetchGroups();
            } else {
                showToast("Error: " + data.message);
            }
        })
        .catch(error => {
            console.error("Error removing group:", error);
            showToast("An error occurred.");
        });
}

function filterGroups() {
    let searchValue = document.getElementById("groupSearch").value.toLowerCase();
    let rows = document.querySelectorAll("#groupTableBody tr");

    rows.forEach(row => {
        let groupName = row.querySelector("td:first-child").textContent.toLowerCase();
        if (groupName.includes(searchValue)) {
            row.style.display = ""; // Show row
        } else {
            row.style.display = "none"; // Hide row
        }
    });
}

function selectGroup(row) {
    if (!row) return;
    document.querySelectorAll("#groupTableBody tr").forEach(r => r.classList.remove("selected-row"));

    row.classList.add("selected-row");

    selectedGroupId = row.getAttribute("data-group-id");
}

async function openEditModal() {
    selectedEditGroupId = null;
    if (!selectedGroupId) {
        showToast("Please select a group!");
        return;
    }
    try {
        selectedEditGroupId = selectedGroupId
        const response = await fetch(`/Groups/GetGroupById/${authToken}&${selectedGroupId}`);
        const groupDataArray = await response.json();
        const groupData = groupDataArray[0];

        if (groupData.eId !== groupData.manager.id && !groupData.isAdmin) {
            showToast("You do not have permissions to edit this group");
            return;
        }

        document.getElementById("editGroupName").value = groupData.gName;
        document.getElementById("editGroupDescription").value = groupData.gDescription;
        document.getElementById("editGroupManager").innerHTML = `
            <option value="">Select a Manager</option>
            <option value="${groupData.manager.id}" selected>${groupData.manager.name}</option>
        `;
        document.getElementById("editSelectedEmployees").innerHTML =
            groupData.members && groupData.members.length
                ? groupData.members.map(member =>
                    `<p>${member.fName} ${member.lName} <button class="remove-btn" onclick="removeMember(${member.eId})">-</button></p>`
                ).join("")
                : "<p>No members added</p>";

        await setEditModel(groupData);

        document.getElementById("editGroupModal").style.display = "flex";
    } catch (error) {
        console.error("Error fetching group data:", error);
    }
}

async function setEditModel(groupData) {

    selectedEditManager = groupData.manager;
    selectedEditMembers = groupData.members || [];

    allEditManagers = await fetchAllAvailableManagers();
    allEditEmployees = await fetchAllEmployees();
    if (groupData.manager && groupData.manager.id) {
        allEditManagers = allEditManagers.filter(manager => manager.id !== groupData.manager.id);
    }
    if (groupData.members && groupData.members.length > 0) {
        allEditEmployees = allEditEmployees.filter(employee =>
            !groupData.members.some(member => member.eId === employee.id)
        );
    }

    document.getElementById("removeEditManagerBtn").style.display = "inline-block";



    populateEditManagerLists(); 
    populateEditLists();        
}

function selectEditManager(id, name) {
    if (selectedEditManager) {
        allEditManagers.push(selectedEditManager);
    }

    selectedEditManager = { id, name };

    document.getElementById("editGroupManager").innerHTML = `
        <option value="">Select a Manager</option>
        <option value="${id}" selected>${name}</option>
    `;

    allEditManagers = allEditManagers.filter(m => m.id !== id);
    document.getElementById("removeEditManagerBtn").style.display = "inline-block";
    populateEditManagerLists();
}

function addEditMember(id, name) {
    if (!selectedEditMembers.some(m => m.id === id)) {
        selectedEditMembers.push({ id, name });
        allEditEmployees = allEditEmployees.filter(e => e.id !== id);
        populateEditLists();
    }
}

function removeEditMember(id) {
    let member = selectedEditMembers.find(m => m.id === id);
    selectedEditMembers = selectedEditMembers.filter(m => m.id !== id);
    if (member) allEditEmployees.push(member);
    populateEditLists();
}

function populateEditLists() {
    document.getElementById("editEmployeeList").innerHTML = allEditEmployees
        .map(employee => `<p>${employee.name} <button class="select-btn" onclick="addEditMember(${employee.id}, '${employee.name}')">+</button></p>`)
        .join("");

    document.getElementById("editSelectedEmployees").innerHTML = selectedEditMembers.length
        ? selectedEditMembers.map(member => `<p>${member.name}<button class="remove-btn" onclick="removeEditMember(${member.id})">-</button></p>`).join("")
        : "<p>No members added</p>";
}

function populateEditManagerLists() {
    let managerDropdown = document.getElementById("editGroupManager");
    let managerList = document.getElementById("editManagerList");

    managerDropdown.innerHTML = `<option value="">Select a Manager</option>`;
    managerList.innerHTML = "";

    allEditManagers.forEach(manager => {
        managerList.innerHTML += `<p>${manager.name} 
            <button class="select-btn" onclick="selectEditManager(${manager.id}, '${manager.name}')">+</button>
        </p>`;
    });

    if (selectedEditManager) {
        managerDropdown.innerHTML = `<option value="${selectedEditManager.id}">${selectedEditManager.name}</option>`;
    }
}

function removeEditManager() {
    if (selectedEditManager) {
        allEditManagers.push(selectedEditManager);
        selectedEditManager = null;

        document.getElementById("editGroupManager").innerHTML = `<option value="">Select a Manager</option>`;

        document.getElementById("removeEditManagerBtn").style.display = "none";
        populateEditManagerLists();
    }
}

function closeEditModal() {
    selectedGroupId = null;
    document.getElementById("editGroupName").value = "";
    document.getElementById("editGroupManager").value = "";
    document.getElementById("editSelectedEmployees").value = "";
    document.getElementById("editGroupDescription").value = "";
    document.getElementById("editGroupModal").style.display = "none";
    
    clearEditErrors();

}

async function saveGroupEdits() {
    if (!validateEditForm()) return;

    const groupData = {
        groupId: selectedEditGroupId,
        groupName: document.getElementById("editGroupName").value.trim(),
        groupDescription: document.getElementById("editGroupDescription").value.trim(),
        managerId: selectedEditManager
            ? selectedEditManager.id
            : parseInt(document.getElementById("editGroupManager").value) || 0,
        memberIds: selectedEditMembers.map(member => member.id)
    };

    console.log("Sending Edited Group Data:", groupData);

    try {
        const response = await fetch(`/Groups/SaveGroupEdits/${authToken}`, {
            method: "POST", 
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(groupData)
        });

        const result = await response.json();

        if (result.success) {
            showToast("Group updated successfully!");
            closeEditModal();
            fetchUserGroups();
            fetchGroups();
        } else {
            alert(`Error: ${result.message}`);
        }
    } catch (error) {
        console.error("Error updating group:", error);
        alert("An error occurred while updating the group.");
    }
}

function validateEditForm() {
    let isValid = true;

    let nameInput = document.getElementById("editGroupName").value.trim();
    let managerInput = document.getElementById("editGroupManager").value.trim();

    clearEditErrors();

    if (nameInput === "") {
        displayEditError("editGroupNameError", "Group name is required.");
        isValid = false;
    }
    if (managerInput === "" || managerInput === "Select a Manager") {
        displayEditError("editGroupManagerError", "Manager selection is required.");
        isValid = false;
    }

    return isValid;
}

function clearEditErrors() {
    let errors = document.querySelectorAll("#editGroupModal .error-message");
    errors.forEach(error => {
        error.style.display = "none";
        error.textContent = "";
    });
}

function filterEditManagers() {
    let searchValue = document.getElementById("editManagerSearch").value.toLowerCase();
    let managerItems = document.querySelectorAll("#editManagerList p");

    managerItems.forEach((item) => {
        let managerName = item.textContent.toLowerCase();
        if (managerName.includes(searchValue)) {
            item.style.display = "flex";
        } else {
            item.style.display = "none";
        }
    });
}

function filterEditEmployees() {
    let searchValue = document.getElementById("editEmployeeSearch").value.toLowerCase();
    let employeeItems = document.querySelectorAll("#editEmployeeList p");

    employeeItems.forEach((item) => {
        let employeeName = item.textContent.toLowerCase();
        if (employeeName.includes(searchValue)) {
            item.style.display = "flex";
        } else {
            item.style.display = "none";
        }
    });
}

function displayEditError(elementId, message) {
    let errorElement = document.getElementById(elementId);
    if (errorElement) {
        errorElement.textContent = message;
        errorElement.style.display = "block";
    }
}

function showToast(message, duration = 3000) {
    const toast = document.getElementById("toast");
    toast.textContent = message;
    toast.style.display = "block";

    void toast.offsetWidth;

    toast.style.opacity = 1;

    setTimeout(() => {
        toast.style.opacity = 0;
        setTimeout(() => {
            toast.style.display = "none";
        }, 500); 
    }, duration);
}
