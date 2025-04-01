function markAccepted(pid, gid) {
    alert("Accepted!")
    var project = document.getElementById(`request-${pid}-${gid}`);
    project.style.display = "none";
}