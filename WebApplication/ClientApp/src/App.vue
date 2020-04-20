<template>
    <div>
        <app-toolbar></app-toolbar>
        <app-todo-list></app-todo-list>
        <app-todo-create></app-todo-create>
    </div>
</template>

<script>
import appToolbar from './components/Toolbar.vue'
import appTodoCreate from './components/TodoCreate.vue'
import appTodoList from './components/TodoList.vue'

export default {
    components: {
        appToolbar,
        appTodoCreate,
        appTodoList
    }
}
</script>