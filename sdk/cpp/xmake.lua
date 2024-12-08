add_rules("mode.debug", "mode.release")

add_requires("cxxopts v3.2.1")
add_requires("glaze v4.0.1")
add_requires("libhv v1.3.3", {configs = {http_server = false}})
add_requires("magic_enum v0.9.7")
add_requires("spdlog v1.15.0", {configs = {std_format = true}})

target("agent")
    set_kind("binary")
    add_packages("cxxopts", "glaze", "libhv", "magic_enum", "spdlog")
    add_includedirs("src")
    add_files("src/**.cc")
    set_languages("cxx23")
    set_exceptions("cxx")
    set_warnings("allextra")

    if is_plat("windows") then
        add_defines("NOMINMAX")
    end

    after_build(function (target)
        os.cp(
            target:targetfile(), 
            path.join(os.projectdir(), "bin", path.filename(target:targetfile()))
        )
    end)
