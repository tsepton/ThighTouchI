<script>
	import Main from '$lib/components/Main.svelte';
</script>

<Main></Main>

<style>
	:global(body) {
		font-family: 'Roboto', sans-serif;
		margin: 0;
		padding: 0;
		height: 100vh;
		background-image: linear-gradient(to right bottom, #fdfffc, #fbfef9, #f9fdf6, #f6fcf4, #f4fbf1);
	}
	:global(.card) {
		background: rgba(255, 255, 255, 0.2);
		border-radius: 16px;
		box-shadow: 0 4px 30px rgba(0, 0, 0, 0.1);
		backdrop-filter: blur(5px);
		-webkit-backdrop-filter: blur(5px);
		border: 1px solid rgba(255, 255, 255, 0.3);
		padding-top: 2rem;
        padding-bottom: 2rem;
	}
</style>
