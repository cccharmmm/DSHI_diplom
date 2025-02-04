function toggleDropdown() {
    const dropdownMenu = document.querySelector('.dropdown-menu');

    if (dropdownMenu.style.display === 'block') {
        dropdownMenu.style.display = 'none'; 
    } else {
        dropdownMenu.style.display = 'block';
    }
}
