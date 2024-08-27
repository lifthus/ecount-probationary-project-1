export class FilterItemDiv extends HTMLElement {
    constructor() {
        super();
    }
    connectedCallback() {
        const content = this.innerHTML.trim() || href;
        this.innerHTML = `
        <div class="flex bg-whitesmoke bd-solid bd-sm p-xs">
            ${content}
        </div>
        `;
    }
}
