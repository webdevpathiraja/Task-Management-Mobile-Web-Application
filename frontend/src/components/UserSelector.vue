<template>
  <div class="mb-6 text-center">
    <label for="user-select" class="block text-m font-medium text-gray-700 mb-2">
      Select User
    </label>
    <select
      id="user-select"
      v-model="selectedUserId"
      @change="$emit('userChanged', selectedUserId)"
      class="block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-200 focus:border-blue-500"
    >
      <option value="">All Users</option>
      <option v-for="user in users" :key="user.id" :value="user.id">
        {{ user.name }} ({{ user.email }})
      </option>
    </select>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import type { User } from '../types';
import { userService } from '../services/api';

const users = ref<User[]>([]);
const selectedUserId = ref<number | ''>('');

const emit = defineEmits<{
  userChanged: [userId: number | '']
}>();

onMounted(async () => {
  try {
    users.value = await userService.getAllUsers();
  } catch (error) {
    console.error('Failed to load users:', error);
  }
});
</script>