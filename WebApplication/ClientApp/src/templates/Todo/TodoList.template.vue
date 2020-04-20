<template>
    <md-list class="md-double-line">
        <md-list-item v-for="todo in todos">
            <md-button class="md-icon-button md-raised md-dense" v-bind:disabled="todo.completed" v-on:click.once="markCompleteTask(todo.id)"><md-icon v-if="todo.completed">checked</md-icon></md-button>
            <div class="md-list-text-container">
                <span v-bind:completed="todo.completed">{{ todo.description }}</span>
                <span>{{ todo.type }}</span>
            </div>
        </md-list-item>
    </md-list>
</template>
<style scoped>
    span[completed] {
        text-decoration: line-through;
    }
</style>