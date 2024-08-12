export class GrayButton extends HTMLElement {
    constructor() {
        super();
    }
    connectedCallback() {
        const buttonId = this.getAttribute("buttonId");
        const content = this.innerHTML.trim() || href;
        this.innerHTML = `
        <button 
            class="bg-whitesmoke bd-lightgray"
            id="${buttonId}"
            type="button"
        >${content}</button>
        `;
    }
}
