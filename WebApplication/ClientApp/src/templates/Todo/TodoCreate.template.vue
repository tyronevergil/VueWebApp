<template>
    <div>
        <md-button class="md-fab md-fab-top-right md-warn" @click="openDialog"><md-icon>add</md-icon></md-button>
        <md-dialog ref="dlgCreateTask">
            <md-dialog-title>New Task</md-dialog-title>
            <md-dialog-content>
                <form>
                    <md-input-container>
                        <label>Description</label>
                        <md-input v-model="description"></md-input>
                    </md-input-container>
                    <md-input-container>
                        <label>Type</label>
                        <md-autocomplete v-model="type" print-attribute="label" :fetch="getTaskTypes" :debounce="200"></md-autocomplete>
                    </md-input-container>
                </form>
            </md-dialog-content>
            <md-dialog-actions>
                <md-button @click="closeDialog">Cancel</md-button>
                <md-button @click="createTask">Create</md-button>
            </md-dialog-actions>
        </md-dialog>
    </div>
</template>
<style scoped>
    div >>> .md-dialog {
        width: 75%;
    }
</style>