export class FilterInputText extends HTMLElement {
    constructor() {
        super();
    }
    connectedCallback() {
        const filterId = this.getAttribute("filterId");
        const filterName = this.getAttribute("filterName");
        const defaultValue = this.getAttribute("defaultValue");
        const content = this.innerHTML.trim() || href;
        this.innerHTML = `
          <div class="flex bg-whitesmoke bd-solid bd-sm p-sm">
            <label
                for="${filterId}" 
                class="w-100px"
            >
                ${content}
            </label>
            <input
                id="${filterId}"
                name="${filterName}"
                placeholder="${content}"
                value="${defaultValue}"
            />
        </div>
        `;
    }
}
