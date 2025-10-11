document.addEventListener('DOMContentLoaded', function () {
    var userDropdownBtn = document.getElementById('userDropdown');
    var dropdownMenu = userDropdownBtn.nextElementSibling;

    userDropdownBtn.addEventListener('click', function (e) {
        e.preventDefault();
        dropdownMenu.classList.toggle('show');
    });

    document.addEventListener('click', function (e) {
        if (!userDropdownBtn.contains(e.target) && !dropdownMenu.contains(e.target)) {
            dropdownMenu.classList.remove('show');
        }
    });
});
