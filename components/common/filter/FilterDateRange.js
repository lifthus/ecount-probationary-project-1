export class FilterDateRange extends HTMLElement {
    constructor() {
        super();
    }
    connectedCallback() {
        const startDateId = this.getAttribute("startDateId");
        const endDateId = this.getAttribute("endDateId");
        const defaultStartDate = new Date(this.getAttribute("defaultStartDate"));
        const defaultStartYear = defaultStartDate.getFullYear() || "";
        const defaultStartMonth = defaultStartDate.getMonth() + 1 || "";
        const defaultStartDay = defaultStartDate.getDate() || "";
        let startDateValue = "";
        if (defaultStartDate.toString() !== "Invalid Date") startDateValue = `${defaultStartYear}-${defaultStartMonth}-${defaultStartDay}`;
        const defaultEndDate = new Date(this.getAttribute("defaultEndDate"));
        const defaultEndYear = defaultEndDate.getFullYear() || "";
        const defaultEndMonth = defaultEndDate.getMonth() + 1 || "";
        const defaultEndDay = defaultEndDate.getDate() || "";
        let endDateValue = "";
        if (defaultEndDate.toString() !== "Invalid Date") endDateValue = `${defaultEndYear}-${defaultEndMonth}-${defaultEndDay}`;
        const content = this.innerHTML.trim() || href;
        this.innerHTML = `
          <div class="flex bg-whitesmoke bd-solid bd-sm p-sm">
            <input
                type="hidden"
                id="${startDateId}"
                value="${startDateValue}"
            />
            <input
                type="hidden"
                id="${endDateId}"
                value="${endDateValue}"
            />
            <label
                class="w-100px"
            >
                ${content}
            </label>
            <select
                id="filter-start-year"
                name="filter-start-year"
            >
                <option></option>
                ${Array.from({ length: 50 }, (_, i) => {
                    const thisYear = new Date().getFullYear() - i;
                    return `<option value=${thisYear} ${thisYear === defaultStartYear ? "selected" : ""}>${new Date().getFullYear() - i}</option>`;
                }).join("")}
            </select>
            /
            <select
                id="filter-start-month"
                name="filter-start-month"
            >
                <option></option>
                ${Array.from({ length: 12 }, (_, i) => {
                    const thisMonth = i + 1;
                    return `<option value=${i + 1} ${thisMonth === defaultStartMonth ? "selected" : ""}>${i + 1}</option>`;
                }).join("")}
            </select>
            /
            <select
                id="filter-start-day"
                name="filter-start-day"
            >
                <option></option>
                ${Array.from({ length: 31 }, (_, i) => {
                    const thisDay = i + 1;
                    return `<option value=${i + 1} ${thisDay === defaultStartDay ? "selected" : ""}>${i + 1}</option>`;
                }).join("")}
            </select>
            ~
            <select
                id="filter-end-year"
                name="filter-end-year"
            >
                <option></option>
                ${Array.from({ length: 50 }, (_, i) => {
                    const thisYear = new Date().getFullYear() - i;
                    return `<option value=${new Date().getFullYear() - i} ${thisYear === defaultEndYear ? "selected" : ""}>${
                        new Date().getFullYear() - i
                    }</option>`;
                }).join("")}
            </select>
            /
            <select
                id="filter-end-month"
                name="filter-end-month"
            >
                <option></option>
                ${Array.from({ length: 12 }, (_, i) => {
                    const thisMonth = i + 1;
                    return `<option value=${i + 1} ${thisMonth === defaultEndMonth ? "selected" : ""}>${i + 1}</option>`;
                }).join("")}
            </select>
            /
            <select
                id="filter-end-day"
                name="filter-end-day"
            >
                <option></option>
                ${Array.from({ length: 31 }, (_, i) => {
                    const thisDay = i + 1;
                    return `<option value=${i + 1} ${thisDay === defaultEndDay ? "selected" : ""}>${i + 1}</option>`;
                }).join("")}
            </select>
        </div>
        `;
        const startDateInput = document.getElementById(startDateId);
        const endDateInput = document.getElementById(endDateId);
        const startYearInput = document.getElementById("filter-start-year");
        const startMonthInput = document.getElementById("filter-start-month");
        const startDayInput = document.getElementById("filter-start-day");
        const endYearInput = document.getElementById("filter-end-year");
        const endMonthInput = document.getElementById("filter-end-month");
        const endDayInput = document.getElementById("filter-end-day");
        startYearInput.addEventListener("change", () => {
            const startDate = `${startYearInput.value || "N"}-${startMonthInput.value || "N"}-${startDayInput.value || "N"}`;
            const isValidDate = new Date(startDate).toString() !== "Invalid Date";
            if (isValidDate) {
                startDateInput.value = startDate;
                console.log(startDate);
            } else {
                startDateInput.value = "";
            }
        });
        startMonthInput.addEventListener("change", () => {
            const startDate = `${startYearInput.value || "N"}-${startMonthInput.value || "N"}-${startDayInput.value || "N"}`;
            const isValidDate = new Date(startDate).toString() !== "Invalid Date";
            if (isValidDate) {
                startDateInput.value = startDate;
            } else {
                startDateInput.value = "";
            }
        });
        startDayInput.addEventListener("change", () => {
            const startDate = `${startYearInput.value || "N"}-${startMonthInput.value || "N"}-${startDayInput.value || "N"}`;
            const isValidDate = new Date(startDate).toString() !== "Invalid Date";
            if (isValidDate) {
                startDateInput.value = startDate;
            } else {
                startDateInput.value = "";
            }
        });
        endYearInput.addEventListener("change", () => {
            const endDate = `${endYearInput.value || "N"}-${endMonthInput.value || "N"}-${endDayInput.value || "N"}`;
            const isValidDate = new Date(endDate).toString() !== "Invalid Date";
            if (isValidDate) {
                endDateInput.value = endDate;
            } else {
                endDateInput.value = "";
            }
        });
        endMonthInput.addEventListener("change", () => {
            const endDate = `${endYearInput.value || "N"}-${endMonthInput.value || "N"}-${endDayInput.value || "N"}`;
            const isValidDate = new Date(endDate).toString() !== "Invalid Date";
            if (isValidDate) {
                endDateInput.value = endDate;
            } else {
                endDateInput.value = "";
            }
        });
        endDayInput.addEventListener("change", () => {
            const endDate = `${endYearInput.value || "N"}-${endMonthInput.value || "N"}-${endDayInput.value || "N"}`;
            const isValidDate = new Date(endDate).toString() !== "Invalid Date";
            if (isValidDate) {
                endDateInput.value = endDate;
            } else {
                endDateInput.value = "";
            }
        });
    }
}
