export class PageNav extends HTMLElement {
    constructor() {
        super();
    }
    connectedCallback() {
        const urlQuery = new URLSearchParams(window.location.search);
        const pageNo = Number(urlQuery.get("pageNo")) || 1;

        const path = this.getAttribute('path') || '';

        let pageSize = Number(this.getAttribute('pageSize')) || 10;
        if (pageSize < 1) pageSize = 1;
        const totalCount = Number(this.getAttribute('totalCount')) || 0;

        const pageAmount = Math.ceil(totalCount / pageSize) || 1;
        const pageArray = Array.from({ length: pageAmount }, (_, i) => i + 1);

        if (pageAmount < pageNo) {
            urlQuery.set('pageNo', `${pageAmount}`);
            window.location.href = path + `?${urlQuery.toString()}`
        }

        const queryString = urlQuery.toString();

        this.innerHTML = 
        `<div class="flex p-sm gap-sm">
            <gray-button buttonId="prev-page-button">< 이전</gray-button>
            ${
        pageArray.map(p => {
            const params = new URLSearchParams(queryString);
            params.set('pageNo', `${p}`);
            params.set('pageSize', `${getSelectedPageSize()}`)

            if (p == pageNo) {
                return `<a id="pageNo-${p}" class="txt-bold" data-page-no="${p}">${p}</a>`
            }
            return `<a id="pageNo-${p}" data-page-no="${p}">${p}</a>`
        }).join('&nbsp;')
            }
            <gray-button buttonId="next-page-button">다음 ></gray-button>
            페이지 당
            <input class="w-50px" id="page-size-input" type="number" value="${pageSize}"/>
            개
        </div>`;

        this.querySelector('#prev-page-button').addEventListener("click", () => {
            const params = new URLSearchParams(queryString);
            const prevPage = 1 < pageNo ? pageNo - 1 : 1;
            params.set('pageNo', `${prevPage}`);
            params.set('pageSize', `${getSelectedPageSize()}`)
            window.location.href = path + `?${params.toString()}`
        });
        this.querySelector('#next-page-button').addEventListener("click", () => {
            const params = new URLSearchParams(queryString);
            const nextPage = pageNo < pageAmount ? pageNo + 1 : pageAmount;
            params.set('pageNo', `${nextPage}`);
            params.set('pageSize', `${getSelectedPageSize()}`)
            window.location.href = path + `?${params.toString()}`
        });
        this.querySelectorAll('[id^="pageNo-"]').forEach(a => {
            a.addEventListener("click", (e) => {
                const params = new URLSearchParams(queryString);
                params.set('pageSize', `${getSelectedPageSize()}`)
                params.set('pageNo', e.target.dataset.pageNo);
                window.location.href = path + `?${params.toString()}`
            });
        });

        function getSelectedPageSize() {
            const pageSizeInput = document.querySelector('#page-size-input');
            if (!pageSizeInput) {
                return pageSize;
            }
            return Number(pageSizeInput.value) || pageSize;
        }
    }
}
