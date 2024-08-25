'use strict'
const switcher = document.querySelector('.btn');

switcher.addEventListener('click', function () {
    document.body.classList.toggle('dark-theme')
    var classname = document.body.classname;
    if (classname.includes('dark-theme')) {
        this.textContent = "Dark";
    }
    else {
        this.textContent = "ight";
    }
    console.log('current class name:' + classname)
})