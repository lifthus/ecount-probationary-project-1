export class SelectedProduct extends HTMLElement {
    constructor() {
        super();
    }
    connectedCallback() {
        const cancelId = this.getAttribute("cancelId");
        const code = this.getAttribute("code");
        const name = this.getAttribute("name");
        this.innerHTML = `
          <div class="flex bdr-sm bd-solid bd-sm bd-lightgray bg-lightblue txt-white txt-sm justify-center align-center">
            ${code}
            |
            <div class="txt-xs">${name}</div>
            <div id="${cancelId}" class="flex justify-center align-center bdr-sm w-20px h-20px hover:cursor-pointer hover:bg-mildblue txt-center">
                🗑
            </div>
          </div>
        `;
    }
}
