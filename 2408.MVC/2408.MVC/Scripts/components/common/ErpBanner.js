import { MAIN_PATH } from "../../constant.js";

export class ErpBanner extends HTMLElement {
    constructor() {
        super();
    }
    connectedCallback() {
        const urlParams = new URLSearchParams(window.location.search);
        const popup = Number(urlParams.get("popup"));
        if (popup) return;
        this.innerHTML = `
          <div 
            class="bdr-md bg-crimson txt-white px-md py-s hover:cursor-pointer"
            onclick="location.href='${MAIN_PATH}'"
          >
            <h1>ERP</h1>
        </div>
        `;
    }
}
