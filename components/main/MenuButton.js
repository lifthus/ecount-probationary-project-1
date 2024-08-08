class MenuButton extends HTMLElement {
    constructor() {
        super();
        this.attachShadow({ mode: "open" });
    }
    connectedCallback() {
        const href = this.getAttribute("href");
        const content = this.innerHTML.trim() || href;

        this.shadowRoot.innerHTML = `
        <button onclick
          <button href="${href}">${content}</a>
        `;
    }
}
customElements.define("menu-button", MenuButton);
