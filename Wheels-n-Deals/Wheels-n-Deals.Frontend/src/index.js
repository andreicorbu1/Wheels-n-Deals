function getContent(fragmentId, callback) {
    var pages = {
        home: "This is the home page. Welcome to my site",
        register: "This is the register page",
        login: "This is the login page",
        contact: "This is the contact page"
    };
    callback(pages[fragmentId]);
}

function loadContent() {
    var contentDiv = this.document.getElementById("app");
    fragmentId = location.hash.substring(1);
    console.log(fragmentId)
    getContent(fragmentId, function(content) {
        contentDiv.innerHTML = content;
    });
}

if(!location.hash) {
    location.hash = "#home";
}

loadContent();

window.addEventListener("hashchange", loadContent);