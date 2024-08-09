import "./main/MenuButton.js";
import "./main/NavBar.js";
class WebApp extends HTMLElement {
    constructor() {
        super();
        this.attachShadow({ mode: "open" });
        console.log("@" + this.innerHTML + "@");
        this.shadowRoot.innerHTML = `
        <div>
        <style>
            @import url("/styles/util.css");
        </style>
        ${this.innerHTML.trim()}
        </div>
        `;
    }
}
customElements.define("web-app", WebApp);
