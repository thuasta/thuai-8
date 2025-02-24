add_rules("mode.debug", "mode.release")

add_requires("cxxopts")
add_requires("glaze")
add_requires("libhv", {configs = {http_server = false}})
add_requires("magic_enum")
add_requires("spdlog")

target("agent")
    set_kind("binary")
    add_packages("cxxopts", "glaze", "libhv", "magic_enum", "spdlog")
    add_includedirs("src")
    add_files("src/**.cpp")
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
