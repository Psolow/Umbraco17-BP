class ArmyBuilder {
    constructor() {
        this.availableUnits = [];
        this.currentArmy = {
            name: '',
            faction: '',
            period: '',
            targetPoints: 1000,
            units: []
        };
        this.init();
    }

    async init() {
        await this.loadUnits();
        this.render();
        this.attachEventListeners();
    }

    async loadUnits() {
        try {
            const response = await fetch('/umbraco/api/armybuilder/getunits');
            if (response.ok) {
                this.availableUnits = await response.json();
            }
        } catch (error) {
            console.error('Failed to load units:', error);
        }
    }

    addUnitToArmy(unitId) {
        const unit = this.availableUnits.find(u => u.id === unitId);
        if (!unit) return;

        const existingUnit = this.currentArmy.units.find(u => u.unitId === unitId);
        if (existingUnit) {
            existingUnit.quantity++;
        } else {
            this.currentArmy.units.push({
                unitId: unit.id,
                name: unit.name,
                quantity: 1,
                pointsCost: unit.pointsCost,
                notes: ''
            });
        }

        this.render();
    }

    removeUnitFromArmy(unitId) {
        const index = this.currentArmy.units.findIndex(u => u.unitId === unitId);
        if (index > -1) {
            this.currentArmy.units.splice(index, 1);
            this.render();
        }
    }

    updateUnitQuantity(unitId, quantity) {
        const unit = this.currentArmy.units.find(u => u.unitId === unitId);
        if (unit) {
            unit.quantity = Math.max(1, parseInt(quantity) || 1);
            this.render();
        }
    }

    getTotalPoints() {
        return this.currentArmy.units.reduce((sum, unit) => sum + (unit.pointsCost * unit.quantity), 0);
    }

    getTotalUnitCount() {
        return this.currentArmy.units.reduce((sum, unit) => sum + unit.quantity, 0);
    }

    async saveArmy() {
        if (!this.currentArmy.name) {
            alert('Please enter an army name');
            return;
        }

        if (this.currentArmy.units.length === 0) {
            alert('Please add at least one unit to your army');
            return;
        }

        try {
            const response = await fetch('/umbraco/api/armybuilder/savearmy', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(this.currentArmy)
            });

            if (response.ok) {
                alert('Army saved successfully!');
                this.resetArmy();
            } else {
                alert('Failed to save army');
            }
        } catch (error) {
            console.error('Failed to save army:', error);
            alert('Failed to save army');
        }
    }

    resetArmy() {
        this.currentArmy = {
            name: '',
            faction: '',
            period: '',
            targetPoints: 1000,
            units: []
        };
        this.render();
    }

    render() {
        const builderTab = document.getElementById('builder-tab');
        if (!builderTab) return;

        const totalPoints = this.getTotalPoints();
        const totalUnits = this.getTotalUnitCount();
        const pointsPercentage = (totalPoints / this.currentArmy.targetPoints) * 100;

        builderTab.innerHTML = `
            <h2 style="margin-bottom: 20px;">Army Builder</h2>

            <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 30px;">
                <div>
                    <div class="card" style="margin-bottom: 20px;">
                        <h3>Army Details</h3>
                        <div style="margin-top: 15px;">
                            <label style="display: block; margin-bottom: 5px; font-weight: 600;">Army Name</label>
                            <input type="text" id="army-name" value="${this.currentArmy.name}"
                                   style="width: 100%; padding: 10px; border: 1px solid #dee2e6; border-radius: 4px; font-size: 1em;"
                                   placeholder="Enter army name">
                        </div>
                        <div style="margin-top: 15px;">
                            <label style="display: block; margin-bottom: 5px; font-weight: 600;">Faction</label>
                            <input type="text" id="army-faction" value="${this.currentArmy.faction}"
                                   style="width: 100%; padding: 10px; border: 1px solid #dee2e6; border-radius: 4px; font-size: 1em;"
                                   placeholder="E.g., British, French, Prussian">
                        </div>
                        <div style="margin-top: 15px;">
                            <label style="display: block; margin-bottom: 5px; font-weight: 600;">Period</label>
                            <input type="text" id="army-period" value="${this.currentArmy.period}"
                                   style="width: 100%; padding: 10px; border: 1px solid #dee2e6; border-radius: 4px; font-size: 1em;"
                                   placeholder="E.g., Napoleonic Wars">
                        </div>
                        <div style="margin-top: 15px;">
                            <label style="display: block; margin-bottom: 5px; font-weight: 600;">Target Points</label>
                            <input type="number" id="army-points" value="${this.currentArmy.targetPoints}"
                                   style="width: 100%; padding: 10px; border: 1px solid #dee2e6; border-radius: 4px; font-size: 1em;"
                                   min="500" step="100">
                        </div>
                    </div>

                    <div class="card">
                        <h3>Available Units</h3>
                        <div style="max-height: 500px; overflow-y: auto; margin-top: 15px;">
                            ${this.availableUnits.map(unit => `
                                <div style="border: 1px solid #dee2e6; padding: 15px; margin-bottom: 10px; border-radius: 6px; cursor: pointer; transition: all 0.2s;"
                                     onmouseover="this.style.background='#f8f9fa'"
                                     onmouseout="this.style.background='white'"
                                     onclick="armyBuilder.addUnitToArmy('${unit.id}')">
                                    <div style="display: flex; justify-content: space-between; align-items: center;">
                                        <div>
                                            <strong>${unit.name}</strong>
                                            <div style="margin-top: 5px;">
                                                <span class="badge badge-${unit.type?.toLowerCase()}">${unit.type}</span>
                                                <span class="badge badge-size">${unit.size}</span>
                                            </div>
                                        </div>
                                        <div style="text-align: right;">
                                            <div style="font-size: 1.5em; font-weight: bold; color: #667eea;">${unit.pointsCost}</div>
                                            <div style="font-size: 0.85em; color: #6c757d;">points</div>
                                        </div>
                                    </div>
                                </div>
                            `).join('')}
                        </div>
                    </div>
                </div>

                <div>
                    <div class="card" style="margin-bottom: 20px; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white;">
                        <h3 style="color: white;">Army Summary</h3>
                        <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 15px; margin-top: 20px;">
                            <div style="background: rgba(255,255,255,0.2); padding: 15px; border-radius: 6px; text-align: center;">
                                <div style="font-size: 2em; font-weight: bold;">${totalPoints}</div>
                                <div style="opacity: 0.9;">Total Points</div>
                            </div>
                            <div style="background: rgba(255,255,255,0.2); padding: 15px; border-radius: 6px; text-align: center;">
                                <div style="font-size: 2em; font-weight: bold;">${totalUnits}</div>
                                <div style="opacity: 0.9;">Total Units</div>
                            </div>
                        </div>
                        <div style="margin-top: 20px;">
                            <div style="display: flex; justify-content: space-between; margin-bottom: 8px;">
                                <span>Points Budget</span>
                                <span>${totalPoints} / ${this.currentArmy.targetPoints}</span>
                            </div>
                            <div style="background: rgba(255,255,255,0.3); border-radius: 10px; height: 20px; overflow: hidden;">
                                <div style="background: ${pointsPercentage > 100 ? '#ff4444' : '#4CAF50'}; height: 100%; width: ${Math.min(pointsPercentage, 100)}%; transition: width 0.3s;"></div>
                            </div>
                            ${pointsPercentage > 100 ? '<div style="color: #ffcccc; margin-top: 8px; font-size: 0.9em;">⚠️ Army exceeds target points!</div>' : ''}
                        </div>
                    </div>

                    <div class="card">
                        <h3>Army List</h3>
                        ${this.currentArmy.units.length === 0 ? `
                            <div style="text-align: center; padding: 40px; color: #6c757d;">
                                <div style="font-size: 3em; margin-bottom: 15px;">📝</div>
                                <p>No units added yet</p>
                                <p style="font-size: 0.9em;">Click on units from the left to add them</p>
                            </div>
                        ` : `
                            <div style="margin-top: 15px;">
                                ${this.currentArmy.units.map(armyUnit => {
                                    const unit = this.availableUnits.find(u => u.id === armyUnit.unitId);
                                    if (!unit) return '';

                                    return `
                                        <div style="border: 1px solid #dee2e6; padding: 15px; margin-bottom: 10px; border-radius: 6px;">
                                            <div style="display: flex; justify-content: space-between; align-items: start;">
                                                <div style="flex: 1;">
                                                    <strong style="font-size: 1.1em;">${unit.name}</strong>
                                                    <div style="margin-top: 5px;">
                                                        <span class="badge badge-${unit.type?.toLowerCase()}">${unit.type}</span>
                                                        <span class="badge badge-size">${unit.size}</span>
                                                    </div>
                                                    <div style="display: flex; gap: 10px; align-items: center; margin-top: 10px;">
                                                        <label style="font-size: 0.9em; color: #6c757d;">Quantity:</label>
                                                        <input type="number" value="${armyUnit.quantity}" min="1" max="99"
                                                               onchange="armyBuilder.updateUnitQuantity('${armyUnit.unitId}', this.value)"
                                                               style="width: 70px; padding: 5px; border: 1px solid #dee2e6; border-radius: 4px;">
                                                    </div>
                                                </div>
                                                <div style="text-align: right;">
                                                    <div style="font-size: 1.3em; font-weight: bold; color: #667eea;">
                                                        ${armyUnit.pointsCost * armyUnit.quantity}
                                                    </div>
                                                    <div style="font-size: 0.85em; color: #6c757d;">${armyUnit.pointsCost} × ${armyUnit.quantity}</div>
                                                    <button onclick="armyBuilder.removeUnitFromArmy('${armyUnit.unitId}')"
                                                            style="margin-top: 10px; padding: 6px 12px; background: #dc3545; color: white; border: none; border-radius: 4px; cursor: pointer; font-size: 0.85em;"
                                                            onmouseover="this.style.background='#c82333'"
                                                            onmouseout="this.style.background='#dc3545'">
                                                        Remove
                                                    </button>
                                                </div>
                                            </div>
                                        </div>
                                    `;
                                }).join('')}
                            </div>
                            <div style="margin-top: 20px; display: flex; gap: 10px;">
                                <button onclick="armyBuilder.saveArmy()" class="btn btn-primary" style="flex: 1;">
                                    Save Army
                                </button>
                                <button onclick="armyBuilder.resetArmy()"
                                        style="padding: 12px 24px; background: #6c757d; color: white; border: none; border-radius: 6px; cursor: pointer; font-weight: 600;"
                                        onmouseover="this.style.background='#5a6268'"
                                        onmouseout="this.style.background='#6c757d'">
                                    Clear
                                </button>
                            </div>
                        `}
                    </div>
                </div>
            </div>
        `;
    }

    attachEventListeners() {
        // Use event delegation for dynamically created elements
        document.addEventListener('input', (e) => {
            if (e.target.id === 'army-name') {
                this.currentArmy.name = e.target.value;
            } else if (e.target.id === 'army-faction') {
                this.currentArmy.faction = e.target.value;
            } else if (e.target.id === 'army-period') {
                this.currentArmy.period = e.target.value;
            } else if (e.target.id === 'army-points') {
                this.currentArmy.targetPoints = parseInt(e.target.value) || 1000;
                this.render();
            }
        });
    }
}

// Initialize when the page loads
let armyBuilder;
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => {
        armyBuilder = new ArmyBuilder();
    });
} else {
    armyBuilder = new ArmyBuilder();
}
